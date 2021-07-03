using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace FunPokemon.YodaApi
{
    public class YodaApiService : IYodaApiService
    {
        private readonly HttpClient _client;

        public YodaApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<YodaApiResponse> TranslateToYodaSpeak(string textToTranslate)
        {
            // This API is _SUPER_ slow, I need to put some caching in here or something
            var response = await _client.GetAsync($"yoda.json?text={HttpUtility.UrlEncode(textToTranslate)}");
            var responseJson = await response.Content.ReadAsStringAsync();
            var yoda = response.IsSuccessStatusCode ? JsonSerializer.Deserialize<YodaApiResponse>(responseJson) : null;
            return yoda;
        }
    }
}
