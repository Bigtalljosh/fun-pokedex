using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace FunPokedex.ShakespeareApi
{
    public class ShakespeareApiService : IShakespeareApiService
    {
        private readonly HttpClient _client;
        private readonly IMemoryCache _cache;

        public ShakespeareApiService(HttpClient client, IMemoryCache cache)
        {
            _client = client;
            _cache = cache;
        }

        public async Task<ShakespeareApiResponse> TranslateToShakespeareSpeak(string textToTranslate)
        {
            // In a production scenario we should never use user input as a cache key!
            var cacheKey = $"yoda-{textToTranslate}";

            if (!_cache.TryGetValue(cacheKey, out ShakespeareApiResponse shakespeareResponse))
            {
                var response = await _client.GetAsync($"shakespearee.json?text={HttpUtility.UrlEncode(textToTranslate)}");
                var responseJson = await response.Content.ReadAsStringAsync();
                var shakespeare = response.IsSuccessStatusCode ? JsonSerializer.Deserialize<ShakespeareApiResponse>(responseJson) : null;

                var cacheOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(60));

                _cache.Set(cacheKey, shakespeare, cacheOptions);
                return shakespeare;
            }

            return shakespeareResponse;
        }
    }
}
