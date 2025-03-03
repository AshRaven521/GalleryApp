using GalleryMobile.DataPersistence.Entities;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GalleryMobile.UnsplashAPI.Model
{
    [Table("photos")]
    public class UnsplashPhoto
    {
        [PrimaryKey]
        [Column("id")]
        public string? Id { get; set; }

        [ForeignKey(typeof(User))]
        [Column("user_id")]
        public int? UserId { get; set; }
        [Column("description")]
        public string? Description { get; set; }
        [Column("url")]
        public Uri? Url { get; set; }
        [Column("is_liked")]
        public bool? IsLiked { get; set; }

        public UnsplashPhoto()
        {
            
        }
    }
}
