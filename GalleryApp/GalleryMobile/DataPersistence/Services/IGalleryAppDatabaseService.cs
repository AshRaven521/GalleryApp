﻿using GalleryMobile.DataPersistence.Entities;
using GalleryMobile.UnsplashAPI.Model;

namespace GalleryMobile.DataPersistence.Services
{
    public interface IGalleryAppDatabaseService
    {
        Task<User?> GetUserByEmailAsync(string email, CancellationToken token);
        Task<User?> GetUserByNickNameAsync(string nickName, CancellationToken token);
        Task SaveUserAsync(User user, CancellationToken token);
        Task<List<ThumbnailPhoto>> GetUserLikedPhotosAsync(User user, CancellationToken token);
        Task<User?> GetLastLoggedInUser(CancellationToken token);
    }
}