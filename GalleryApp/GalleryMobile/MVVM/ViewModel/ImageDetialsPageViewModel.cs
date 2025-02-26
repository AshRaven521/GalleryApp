using CommunityToolkit.Mvvm.ComponentModel;
using GalleryMobile.UnsplashAPI.Model;

namespace GalleryMobile.MVVM.ViewModel
{
    public partial class ImageDetialsViewModel : ObservableObject
    {

        private UnsplashPhoto photo;

        public ImageDetialsViewModel(UnsplashPhoto photo)
        {
            this.photo = photo;
        }

        public UnsplashPhoto Photo
        {
            get
            {
                return photo;
            }
            set
            {
                if (photo == value)
                {
                    return;
                }
                photo = value;
                OnPropertyChanged(nameof(Photo));
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {

            var navigatedPhoto = query["Photo"] as UnsplashPhoto;
            photo.Url = navigatedPhoto.Url;
            photo.Description = navigatedPhoto.Description;
            photo.Id = navigatedPhoto.Id;
        }
    }
}
