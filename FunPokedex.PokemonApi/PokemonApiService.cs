using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FunPokedex.PokemonApi
{
    public class PokemonApiService : IPokemonApiService
    {
        private readonly HttpClient _client;

        public PokemonApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<PokemonApiResponse> GetPokemonDetails(string pokemonNameOrId)
        {
            var response = await _client.GetAsync($"v2/pokemon-species/{pokemonNameOrId}");
            var responseJson = await response.Content.ReadAsStringAsync();
            var pokemon = response.IsSuccessStatusCode ? JsonSerializer.Deserialize<PokemonApiResponse>(responseJson) : null;
            return pokemon;
        }
    }
}
