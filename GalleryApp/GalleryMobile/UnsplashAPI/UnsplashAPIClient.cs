using GalleryMobile.UnsplashAPI.Exceptions;
using GalleryMobile.UnsplashAPI.Model;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace GalleryMobile.UnsplashAPI
{
    public class UnsplashAPIClient
    {
        private readonly string KEYS_FILE_NAME = "keys.json";
        private readonly string BASE_PHOTOS_URI = "https://api.unsplash.com/photos";

        public async Task<string> GetAccessKeyAsync(CancellationToken cancellationToken)
        {
            bool isFileExists = await FileSystem.AppPackageFileExistsAsync(KEYS_FILE_NAME);
            if (!isFileExists)
            {
                return string.Empty;
            }

            using var stream = await FileSystem.OpenAppPackageFileAsync(KEYS_FILE_NAME);
            using var reader = new StreamReader(stream);

            string json = await reader.ReadToEndAsync(cancellationToken);

            var accessKey = JsonSerializer.Deserialize<ApiKey>(json);

            if (string.IsNullOrWhiteSpace(accessKey?.Key))
            {
                return string.Empty;
            }

            return accessKey.Key;
        }

        public async Task<List<UnsplashPhoto>> GetPhotosAsync(CancellationToken cancellationToken, int page = 1)
        {
            string accessKey = await GetAccessKeyAsync(cancellationToken);

            Dictionary<string, int> queryParams = new Dictionary<string, int>
            {
                {"per_page", 30},
                {"page", page }
            };

            var uriBuilder = new UriBuilder(BASE_PHOTOS_URI)
            {
                Query = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={kvp.Value}"))
            };


            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Client-ID", accessKey);

            var respone = await httpClient.GetAsync(uriBuilder.Uri, cancellationToken);

            if (!respone.IsSuccessStatusCode)
            {
                throw new UnsplashAPIException("There are problems with getting data from Unsplash API");
            }

            string content = await respone.Content.ReadAsStringAsync(cancellationToken);

            var json = JsonObject.Parse(content);

            var unsplashPhotos = new List<UnsplashPhoto>();

            if (json is JsonArray photos)
            {
                foreach (var photo in photos)
                {
                    string id = photo["id"].GetValue<string>();
                    string description = photo["alt_description"].GetValue<string>();
                    string url = photo["urls"]["regular"].GetValue<string>();

                    var unsplahPhoto = new UnsplashPhoto
                    {
                        Id = id,
                        Description = description,
                        Url = new Uri(url)
                    };

                    unsplashPhotos.Add(unsplahPhoto);
                }
            }

            return unsplashPhotos;
        }
    }
}
