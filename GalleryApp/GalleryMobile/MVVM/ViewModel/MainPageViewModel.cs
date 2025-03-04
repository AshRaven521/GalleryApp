using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalleryMobile.DataPersistence.Entities;
using GalleryMobile.DataPersistence.Services;
using GalleryMobile.MVVM.View.Pages;
using GalleryMobile.UnsplashAPI;
using GalleryMobile.UnsplashAPI.Exceptions;
using GalleryMobile.UnsplashAPI.Model;
using System.Collections.ObjectModel;

namespace GalleryMobile.MVVM.ViewModel
{
    public partial class MainPageViewModel : ObservableObject, IQueryAttributable
    {
        private int pageNumber = 1;

        private readonly IUnsplashAPIClient client;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly IGalleryAppDatabaseService database;
        private List<UnsplashPhoto> downloadedPhotos;

        public MainPageViewModel(IUnsplashAPIClient client,
                                 CancellationTokenSource cancellationTokenSource,
                                 IGalleryAppDatabaseService database)
        {
            this.client = client;
            this.cancellationTokenSource = cancellationTokenSource;
            this.database = database;
            Connectivity.ConnectivityChanged += async (s, e) => await CheckNetworkAccess(s, e);
        }

        private User currentUser;

        public User CurrentUser
        {
            get
            {
                return currentUser;
            }
            set
            {
                if (currentUser == value)
                {
                    return;
                }
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
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

        /* NOTE: this property change when need to hide like button */
        private bool isLikeVisible;
        public bool IsLikeVisible
        {
            get
            {
                return isLikeVisible;
            }
            set
            {
                if (isLikeVisible == value)
                {
                    return;
                }
                isLikeVisible = value;
                OnPropertyChanged(nameof(IsLikeVisible));
            }
        }

        [RelayCommand]
        public async Task GetPhotosAsync()
        {
            IsLikeVisible = false;
            try
            {

                downloadedPhotos = await client.GetPhotosAsync(cancellationTokenSource.Token);

                var likedByUserPhotos = await database.GetUserLikedPhotosAsync(CurrentUser, cancellationTokenSource.Token);

                if (likedByUserPhotos.Any())
                {

                    foreach (var downloadPhoto in downloadedPhotos)
                    {
                        foreach (var userLikedPhoto in likedByUserPhotos)
                        {
                            if (downloadPhoto.Id == userLikedPhoto.Id)
                            {
                                downloadPhoto.IsLiked = userLikedPhoto.IsLiked;
                            }
                        }
                    }
                }


                UnsplashPhotos = new ObservableCollection<UnsplashPhoto>(downloadedPhotos);

                IsLikeVisible = true;
            }
            catch (UnsplashAPIException)
            {
                await Shell.Current.DisplayAlert("Error", "Erorr ocured while downloading data from Unsplash API!", "Ok");
            }
        }

        [RelayCommand]
        public async Task RemainingItemsThresholdReachedAsync()
        {
            IsLikeVisible = false;
            // Firstly increment because first page loaded with application loaded (or button pressed)
            pageNumber++;
            try
            {
                downloadedPhotos = await client.GetPhotosAsync(cancellationTokenSource.Token, pageNumber);
                foreach (var photo in downloadedPhotos)
                {
                    UnsplashPhotos.Add(photo);
                }
                IsLikeVisible = true;
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

            await Shell.Current.GoToAsync(nameof(ImageDetailsPage), navigationParameter);

        }

        [RelayCommand]
        public async Task LikePhotoAsync(UnsplashPhoto likePhoto)
        {
            var foundPhoto = UnsplashPhotos.FirstOrDefault(x => x.Id == likePhoto.Id);

            if (foundPhoto.IsLiked.Value)
            {
                foundPhoto.UserId = null;
                foundPhoto.IsLiked = false;
                CurrentUser.LikedPhotos.Remove(foundPhoto);
            }
            else
            {
                foundPhoto.UserId = CurrentUser.Id;
                foundPhoto.IsLiked = true;
                CurrentUser.LikedPhotos.Add(foundPhoto);
            }

            await database.SavePhotoAsync(foundPhoto, cancellationTokenSource.Token);
            await database.SaveUserAsync(CurrentUser, cancellationTokenSource.Token);
        }

        [RelayCommand]
        public async Task LogOutAsync()
        {
            CurrentUser.IsLoggedIn = false;
            await database.SaveUserAsync(CurrentUser, cancellationTokenSource.Token);

            var navParams = new Dictionary<string, object>
            {
                {"IsFirstLoad",  false}
            };

            await Shell.Current.GoToAsync($"///{nameof(LogInPage)}", navParams);
        }
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            CurrentUser = (User)query["CurrentUser"];
        }

        private async Task CheckNetworkAccess(object? sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                IsLikeVisible = false;

                await Shell.Current.DisplayAlert("Internet", "Internet Connection Available.", "Ok");
                downloadedPhotos = await client.GetPhotosAsync(cancellationTokenSource.Token, pageNumber);
                UnsplashPhotos = new ObservableCollection<UnsplashPhoto>(downloadedPhotos);

                IsLikeVisible = true;
            }
            else
            {
                IsLikeVisible = false;

                await Shell.Current.DisplayAlert("Internet", "No Internet Connection.", "Ok");
            }
        }

    }
}
