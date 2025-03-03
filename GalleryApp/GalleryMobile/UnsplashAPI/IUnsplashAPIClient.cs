using GalleryMobile.UnsplashAPI.Model;

namespace GalleryMobile.UnsplashAPI
{
    public interface IUnsplashAPIClient
    {
        Task<string> GetAccessKeyAsync(CancellationToken cancellationToken);
        Task<List<UnsplashPhoto>> GetPhotosAsync(CancellationToken cancellationToken, int page = 1);
    }
}