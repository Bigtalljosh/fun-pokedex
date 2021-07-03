using Microsoft.Extensions.Caching.Memory;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FunPokedex.PokemonApi
{
    public class PokemonApiService : IPokemonApiService
    {
        private readonly HttpClient _client;
        private readonly IMemoryCache _cache;

        public PokemonApiService(HttpClient client, IMemoryCache cache)
        {
            _client = client;
            _cache = cache;
        }

        public async Task<PokemonApiResponse> GetPokemonDetails(string pokemonNameOrId)
        {
            var hashedPokemon = Convert.ToBase64String(Encoding.UTF8.GetBytes(pokemonNameOrId));
            var cacheKey = $"pokemon-{hashedPokemon}";

            if (!_cache.TryGetValue(cacheKey, out PokemonApiResponse pokemonResponse))
            {
                var response = await _client.GetAsync($"v2/pokemon-species/{pokemonNameOrId}");
                var responseJson = await response.Content.ReadAsStringAsync();
                var pokemon = response.IsSuccessStatusCode ? JsonSerializer.Deserialize<PokemonApiResponse>(responseJson) : null;

                var cacheOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(60));

                _cache.Set(cacheKey, pokemon, cacheOptions);
                return pokemon;
            }

            return pokemonResponse;
        }
    }
}
