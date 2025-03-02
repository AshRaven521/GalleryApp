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

        private List<UnsplashPhoto> downloadedPhotos;

        public MainPageViewModel(UnsplashAPIClient client,
                                 CancellationTokenSource cancellationTokenSource)
        {
            this.client = client;
            this.cancellationTokenSource = cancellationTokenSource;
            Connectivity.ConnectivityChanged += async (s, e) => await CheckNetworkAccess(s, e);
        }

        private async Task CheckNetworkAccess(object? sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet", "Internet Connection Available.", "Ok");
                downloadedPhotos = await client.GetPhotosAsync(cancellationTokenSource.Token, pageNumber);
                UnsplashPhotos = new ObservableCollection<UnsplashPhoto>(downloadedPhotos);
            }
            else
            {
                await Shell.Current.DisplayAlert("Internet", "No Internet Connection.", "Ok");
            }
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

        [RelayCommand]
        public async Task GetPhotosAsync()
        {
            try
            {
                downloadedPhotos = await client.GetPhotosAsync(cancellationTokenSource.Token);

                UnsplashPhotos = new ObservableCollection<UnsplashPhoto>(downloadedPhotos);
            }
            catch (UnsplashAPIException)
            {
                await Shell.Current.DisplayAlert("Error", "Erorr ocured while downloading data from Unsplash API!", "Ok");
            }
        }

        [RelayCommand]
        public async Task RemainingItemsThresholdReachedAsync()
        {
            // Firstly increment because first page loaded with application loaded (or button pressed)
            pageNumber++;
            try
            {
                downloadedPhotos = await client.GetPhotosAsync(cancellationTokenSource.Token, pageNumber);
                foreach (var photo in downloadedPhotos)
                {
                    UnsplashPhotos.Add(photo);
                }
            }
            catch (UnsplashAPIException)
            {
                await Shell.Current.DisplayAlert("Error", "Erorr ocured while downloading data from Unsplash API!", "Ok");
            }
        }

        [RelayCommand]
        public async Task OpenImageDetailsAsync(UnsplashPhoto commandParameterPhoto)
        {

            var navigationParameter = new Dictionary<string, object>
            {
                {"Photo", commandParameterPhoto },
                {"OtherPhotos", UnsplashPhotos }
            };

            await Shell.Current.GoToAsync(nameof(ImageDetails), navigationParameter);

        }
    }
}
