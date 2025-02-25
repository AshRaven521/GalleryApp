using System.Text.Json.Serialization;

namespace GalleryMobile.UnsplashAPI.Model
{
    public class ApiKey
    {
        [JsonPropertyName("access_key")]
        public string? Key { get; set; }
    }
}
