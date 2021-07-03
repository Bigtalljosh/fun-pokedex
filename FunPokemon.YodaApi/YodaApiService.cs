﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace FunPokemon.YodaApi
{
    public class YodaApiService : IYodaApiService
    {
        private readonly HttpClient _client;
        private readonly IMemoryCache _cache;

        public YodaApiService(HttpClient client, IMemoryCache cache)
        {
            _client = client;
            _cache = cache;
        }

        public async Task<YodaApiResponse> TranslateToYodaSpeak(string textToTranslate)
        {
            // In a production scenario we should never use user input as a cache key!
            var cacheKey = $"yoda-{textToTranslate}";

            if (!_cache.TryGetValue(cacheKey, out YodaApiResponse yodaResponse))
            {
                var response = await _client.GetAsync($"yoda.json?text={HttpUtility.UrlEncode(textToTranslate)}");
                var responseJson = await response.Content.ReadAsStringAsync();
                var yoda = response.IsSuccessStatusCode ? JsonSerializer.Deserialize<YodaApiResponse>(responseJson) : null;

                var cacheOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(60));

                _cache.Set(cacheKey, yoda, cacheOptions);
                return yoda;
            }

            return yodaResponse;
        }
    }
}
