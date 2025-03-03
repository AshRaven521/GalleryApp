using GalleryMobile.DataPersistence.Entities;
using GalleryMobile.UnsplashAPI.Model;
using SQLite;

namespace GalleryMobile.DataPersistence
{
    public class GalleryAppDatabase : IGalleryAppDatabase
    {
        private SQLiteAsyncConnection connection;

        public GalleryAppDatabase()
        {

        }

        private async Task InitAsync()
        {
            if (connection != null)
            {
                return;
            }

            connection = new SQLiteAsyncConnection(DataConstants.DataBasePath, DataConstants.Flags);
            await connection.CreateTableAsync<UnsplashPhoto>();
            await connection.CreateTableAsync<User>();
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            await InitAsync();
            var user = await connection.Table<User>().Where(x => x.Email == email).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetUserByNickNameAsync(string nickName)
        {
            await InitAsync();
            var user = await connection.Table<User>().Where(x => x.UserName == nickName).FirstOrDefaultAsync();
            return user;
        }

        public async Task<int> SaveUserAsync(User user)
        {
            await InitAsync();

            if (user.Id != 0)
            {
                return await connection.UpdateAsync(user);
            }
            return await connection.InsertAsync(user);
        }

        public async Task<List<UnsplashPhoto>> GetLikedPhotosAsync()
        {
            await InitAsync();

            var likedPhotos = await connection.Table<UnsplashPhoto>().Where(photo => photo.IsLiked == true).ToListAsync();
            return likedPhotos;
        }

        public async Task<int> SavePhotoAsync(UnsplashPhoto photo)
        {
            await InitAsync();

            if (!string.IsNullOrWhiteSpace(photo.Id))
            {
                return await connection.UpdateAsync(photo);
            }
            return await connection.InsertAsync(photo);
        }

    }
}
