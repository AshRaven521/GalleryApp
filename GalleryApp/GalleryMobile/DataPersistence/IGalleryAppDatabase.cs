using GalleryMobile.DataPersistence.Entities;
using GalleryMobile.UnsplashAPI.Model;

namespace GalleryMobile.DataPersistence
{
    public interface IGalleryAppDatabase
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByNickNameAsync(string nickName);
        Task<int> SaveUserAsync(User user);
        Task<List<UnsplashPhoto>> GetLikedPhotosAsync();
        Task<int> SavePhotoAsync(UnsplashPhoto photo);
    }
}