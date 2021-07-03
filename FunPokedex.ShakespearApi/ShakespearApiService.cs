using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace FunPokedex.ShakespearApi
{
    public class ShakespearApiService : IShakespearApiService
    {
        private readonly HttpClient _client;

        public ShakespearApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<ShakespearApiResponse> TranslateToShakespearSpeak(string textToTranslate)
        {
            // This API is _SUPER_ slow, I need to put some caching in here or something
            var response = await _client.GetAsync($"shakespeare.json?text={HttpUtility.UrlEncode(textToTranslate)}");
            var responseJson = await response.Content.ReadAsStringAsync();
            var shakespear = response.IsSuccessStatusCode ? JsonSerializer.Deserialize<ShakespearApiResponse>(responseJson) : null;
            return shakespear;
        }
    }
}
