using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalleryMobile.UnsplashAPI.Model;
using System.Collections.ObjectModel;

namespace GalleryMobile.MVVM.ViewModel
{
    public partial class ImageDetialsViewModel : ObservableObject, IQueryAttributable
    {

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

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            TappedPhoto = (UnsplashPhoto)query["Photo"];
            NavigatedPhotos = (ObservableCollection<UnsplashPhoto>)query["OtherPhotos"];

        }
    }
}
