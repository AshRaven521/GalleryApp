using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace GalleryMobile.UnsplashAPI.Model
{
    public class UnsplashPhoto : ObservableObject
    {
        public string? Id { get; set; }

        public int? UserId { get; set; }
        public string? Description { get; set; }
        public Uri? Url { get; set; }
        [NotMapped]
        private bool? isLiked;
        public bool? IsLiked
        {
            get
            {
                return isLiked;
            }
            set
            {
                if (isLiked == value)
                {
                    return;
                }
                isLiked = value;
                OnPropertyChanged(nameof(IsLiked));

            }
        }

        public UnsplashPhoto()
        {

        }
    }
}
