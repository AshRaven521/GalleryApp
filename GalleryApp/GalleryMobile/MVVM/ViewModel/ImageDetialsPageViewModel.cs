using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GalleryMobile.UnsplashAPI.Model;

namespace GalleryMobile.MVVM.ViewModel
{
    public partial class ImageDetialsViewModel : ObservableObject, IQueryAttributable
    {

        private UnsplashPhoto? photo;

        public UnsplashPhoto? Photo
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
            Photo = (UnsplashPhoto)query["Photo"];
        }

    }
}
