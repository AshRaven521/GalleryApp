using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalleryMobile.DataPersistence.Entities;
using GalleryMobile.DataPersistence.Services;
using GalleryMobile.Mappers;
using GalleryMobile.UnsplashAPI.Model;
using System.Collections.ObjectModel;

namespace GalleryMobile.MVVM.ViewModel
{
    public partial class ImageDetialsViewModel : ObservableObject, IQueryAttributable
    {
        private readonly IGalleryAppDatabaseService database;
        private readonly CancellationTokenSource cancellationTokenSource;

        public ImageDetialsViewModel(IGalleryAppDatabaseService database,
                                    CancellationTokenSource cancellationTokenSource)
        {
            this.database = database;
            this.cancellationTokenSource = cancellationTokenSource;
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

        private UnsplashPhoto? tappedPhoto;

        public UnsplashPhoto? TappedPhoto
        {
            get
            {
                return tappedPhoto;
            }
            set
            {
                if (tappedPhoto == value)
                {
                    return;
                }
                tappedPhoto = value;
                OnPropertyChanged(nameof(TappedPhoto));
            }
        }

        private ObservableCollection<UnsplashPhoto>? navigatedPhotos;
        public ObservableCollection<UnsplashPhoto>? NavigatedPhotos
        {
            get
            {
                return navigatedPhotos;
            }
            set
            {
                if (navigatedPhotos == value)
                {
                    return;
                }
                navigatedPhotos = value;
                OnPropertyChanged(nameof(NavigatedPhotos));
            }
        }

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
        public async Task LikePhotoAsync(UnsplashPhoto likePhoto)
        {
            var foundPhoto = NavigatedPhotos.First(x => x.ApiId == likePhoto.ApiId);

            var thumbPhoto = PhotoMapper.MapUnsplashToThumbnail(foundPhoto);

            /* NOTE: If photo is liked(is in liked photos of user object) */
            if (CurrentUser.LikedPhotos.Any(ph => ph.ApiId == thumbPhoto.ApiId))
            {
                foundPhoto.IsLiked = false;
                CurrentUser.LikedPhotos.Remove(thumbPhoto);
            }
            else
            {
                foundPhoto.IsLiked = true;
                CurrentUser.LikedPhotos.Add(thumbPhoto);
            }

            await database.SaveUserAsync(CurrentUser, cancellationTokenSource.Token);

        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            TappedPhoto = (UnsplashPhoto)query["Photo"];
            NavigatedPhotos = (ObservableCollection<UnsplashPhoto>)query["OtherPhotos"];
            CurrentUser = (User)query["CurrentUser"];
        }
    }
}
