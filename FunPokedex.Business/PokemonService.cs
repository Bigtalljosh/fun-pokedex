using FunPokedex.PokemonApi;
using System.Threading.Tasks;

namespace FunPokedex.Business
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonApiService _pokemonApiService;

        public PokemonService(IPokemonApiService pokemonApiService)
        {
            _pokemonApiService = pokemonApiService;
        }

        public async Task<Pokemon> Get(string pokemonNameOrId)
        {
            var sanitisedInput = SanitiseInput(pokemonNameOrId);
            var response = new Pokemon();
            response.MapDetails(await _pokemonApiService.GetPokemonDetails(sanitisedInput))
                    .MapSpecies(await _pokemonApiService.GetPokemonSpeciesDetails(sanitisedInput));
            return response;
        }

        private static string SanitiseInput(string pokemonNameOrId)
        {
            return pokemonNameOrId
                .Replace("'", "")
                .Replace(" ", "-")
                .Replace(".", "")
                .ToLower();
        }
    }
}
