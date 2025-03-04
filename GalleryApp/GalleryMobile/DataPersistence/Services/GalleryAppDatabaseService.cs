using GalleryMobile.DataPersistence.Entities;
using GalleryMobile.UnsplashAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace GalleryMobile.DataPersistence.Services
{
    public class GalleryAppDatabaseService : IGalleryAppDatabaseService
    {
        private readonly GalleryAppContext context;

        public GalleryAppDatabaseService(GalleryAppContext context)
        {
            this.context = context;
        }

        public async Task<User?> GetUserByEmailAsync(string email, CancellationToken token)
        {
            var user = await context.Users.Where(x => x.Email == email).FirstOrDefaultAsync(token);

            return user;
        }

        public async Task<User?> GetLastLoggedInUser(CancellationToken token)
        {
            var users = await context.Users.ToListAsync(token);

            if (!users.Any())
            {
                return null;
            }

            var lastLoggedUser = users.Aggregate((s, a) => s.LastLoggedInDate < a.LastLoggedInDate ? s : a);

            return lastLoggedUser;
        }

        public async Task<User?> GetUserByNickNameAsync(string nickName, CancellationToken token)
        {
            var user = await context.Users.Where(x => x.UserName == nickName).FirstOrDefaultAsync(token);
            return user;
        }

        public async Task SaveUserAsync(User newUser, CancellationToken token)
        {
            var dbUser = await context.Users.FirstOrDefaultAsync(x => x.Id == newUser.Id, token);

            if (dbUser == null)
            {
                await context.Users.AddAsync(newUser, token);
                await context.SaveChangesAsync(token);
                return;
            }

            dbUser.UserName = newUser.UserName;
            dbUser.LikedPhotos = newUser.LikedPhotos;
            dbUser.Email = newUser.Email;
            dbUser.Password = newUser.Password;
            dbUser.IsLoggedIn = newUser.IsLoggedIn;
            dbUser.LastLoggedInDate = newUser.LastLoggedInDate;

            await context.SaveChangesAsync(token);
        }

        public async Task<IEnumerable<UnsplashPhoto>> GetUserLikedPhotosAsync(User user, CancellationToken token)
        {
            var userDb = await context.Users.FirstAsync(u => u.Id == user.Id, token);


            return userDb.LikedPhotos;
        }

        public async Task SavePhotoAsync(UnsplashPhoto newPhoto, CancellationToken token)
        {
            var dbPhoto = await context.Photos.FirstOrDefaultAsync(x => x.Id == newPhoto.Id, token);

            if (dbPhoto == null)
            {
                await context.Photos.AddAsync(newPhoto, token);
                await context.SaveChangesAsync(token);
                return;
            }

            dbPhoto.Url = newPhoto.Url;
            dbPhoto.UserId = newPhoto.UserId;
            dbPhoto.IsLiked = newPhoto.IsLiked;
            dbPhoto.Description = newPhoto.Description;

            await context.SaveChangesAsync(token);
        }


    }
}
