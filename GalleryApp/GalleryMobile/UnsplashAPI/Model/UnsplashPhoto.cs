namespace GalleryMobile.UnsplashAPI.Model
{
    public class UnsplashPhoto
    {
        public required string Id { get; set; }
        public required string Description { get; set; }
        public required Uri Url { get; set; }
    }
}
