using GalleryMobile.UnsplashAPI.Model;
using System.Text.Json;

namespace GalleryMobile.UnsplashAPI
{
    public class UnsplashAPIClient
    {
        private readonly string KEYS_FILE_NAME = "keys.json";
        private readonly string BASE_PHOTOS_URI = "https://api.unsplash.com/photos";

        public async Task<string> GetAccessKey()
        {
            bool isFileExists = await FileSystem.AppPackageFileExistsAsync(KEYS_FILE_NAME);
            if (!isFileExists)
            {
                return string.Empty;
            }

            using var stream = await FileSystem.OpenAppPackageFileAsync(KEYS_FILE_NAME);
            using var reader = new StreamReader(stream);

            string json = await reader.ReadToEndAsync();

            var accessKey = JsonSerializer.Deserialize<ApiKey>(json);

            if (string.IsNullOrWhiteSpace(accessKey?.Key))
            {
                return string.Empty;
            }

            return accessKey.Key;
        }
    }
}
