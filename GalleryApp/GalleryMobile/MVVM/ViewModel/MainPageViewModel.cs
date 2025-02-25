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
        private readonly UnsplashAPIClient client = new UnsplashAPIClient();
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private int pageNumber = 1;

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


        /* NOTE: Create empty constructor for page binding context. Page binding context can't take injectable constructor, so DI can't be released =( */
        public MainPageViewModel()
        {

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
    }
}
