using GalleryMobile.DataPersistence.Entities;
using GalleryMobile.UnsplashAPI.Model;

namespace GalleryMobile.Mappers
{
    public static class PhotoMapper
    {
        public static ThumbnailPhoto MapUnsplashToThumbnail(UnsplashPhoto unsplashPhoto)
        {
            var thumbPhoto = new ThumbnailPhoto();

            thumbPhoto.ApiId = unsplashPhoto.ApiId;
            thumbPhoto.Description = unsplashPhoto.Description;
            thumbPhoto.Url = unsplashPhoto.Url;
            thumbPhoto.Users = unsplashPhoto.Users;

            return thumbPhoto;
        }
    }
}
