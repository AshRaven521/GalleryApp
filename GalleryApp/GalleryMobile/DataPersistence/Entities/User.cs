using GalleryMobile.UnsplashAPI.Model;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GalleryMobile.DataPersistence.Entities
{
    [Table("users")]
    public class User
    {
        [PrimaryKey]
        [AutoIncrement]
        [Column("id")]
        public int Id { get; set; }

        [Column("user_name")]
        public string UserName { get; set; } = string.Empty;

        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [OneToMany]
        [Column("liked_photos")]
        public List<UnsplashPhoto>? LikedPhotos { get; set; }

        public User()
        {

        }

        public User(string userName, string password, string email)
        {
            UserName = userName;
            Password = password;
            Email = email;
        }
    }
}
