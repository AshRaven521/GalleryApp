using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalleryMobile.MVVM.View.Pages;
using GalleryMobile.UnsplashAPI;
using GalleryMobile.UnsplashAPI.Exceptions;
using GalleryMobile.UnsplashAPI.Model;
using System.Collections.ObjectModel;

namespace GalleryMobile.MVVM.ViewModel
{
    public partial class MainPageViewModel : ObservableObject
    {
        private int pageNumber = 1;

        private readonly UnsplashAPIClient client;
        private readonly CancellationTokenSource cancellationTokenSource;

        public MainPageViewModel(UnsplashAPIClient client,
                                 CancellationTokenSource cancellationTokenSource)
        {
            this.client = client;
            this.cancellationTokenSource = cancellationTokenSource;
        }

        private ObservableCollection<UnsplashPhoto> unsplashPhotos;

        public ObservableCollection<UnsplashPhoto> UnsplashPhotos
        {
            get
            {
                return unsplashPhotos;
            }
            set
            {
                if (unsplashPhotos == value)
                {
                    return;
                }
                unsplashPhotos = value;
                OnPropertyChanged(nameof(UnsplashPhotos));
            }
        }

        private UnsplashPhoto selectedPhoto;
        public UnsplashPhoto SelectedPhoto
        {
            get
            {
                return selectedPhoto;
            }
            set
            {
                if (selectedPhoto == value)
                {
                    return;
                }
                selectedPhoto = value;
                OnPropertyChanged(nameof(SelectedPhoto));
            }
        }

        [RelayCommand]
        public async Task GetPhotosAsync()
        {
            List<UnsplashPhoto> photos;
            try
            {
                photos = await client.GetPhotosAsync(cancellationTokenSource.Token);

                UnsplashPhotos = new ObservableCollection<UnsplashPhoto>(photos);
            }
            catch (UnsplashAPIException ex)
            {

            }
        }

        [RelayCommand]
        public async Task RemainingItemsThresholdReachedAsync()
        {
            List<UnsplashPhoto> photos;
            // Firstly increment because first page loaded with application loaded (or button pressed)
            pageNumber++;
            try
            {
                photos = await client.GetPhotosAsync(cancellationTokenSource.Token, pageNumber);
                foreach (var photo in photos)
                {
                    UnsplashPhotos.Add(photo);
                }
            }
            catch (UnsplashAPIException ex)
            {

            }
        }

        [RelayCommand]
        public async Task OpenImageDetailsAsync()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                {"Photo", selectedPhoto }
            };

            await Shell.Current.GoToAsync(nameof(ImageDetails), navigationParameter);

        }
    }
}
