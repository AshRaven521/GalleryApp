using CommunityToolkit.Mvvm.ComponentModel;
using GalleryMobile.DataPersistence.Entities;
namespace GalleryMobile.UnsplashAPI.Model
{
    public class UnsplashPhoto : ObservableObject
    {
        public string ApiId { get; set; } = string.Empty;
        public List<User> Users { get; set; } = new List<User>();
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public string? Description { get; set; }
        public Uri? Url { get; set; }

        private bool isLiked = false;
        public bool IsLiked
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
    }
}
