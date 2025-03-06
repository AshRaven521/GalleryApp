namespace GalleryMobile.DataPersistence.Entities
{
    public class ThumbnailPhoto
    {
        public int Id { get; set; }
        public string ApiId { get; set; } = string.Empty;
        public List<User> Users { get; set; } = new List<User>();
        public string? Description { get; set; }
        public Uri? Url { get; set; }

    }
}
