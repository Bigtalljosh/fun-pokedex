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
            var pokemonDetails = await _pokemonApiService.GetPokemonDetails(sanitisedInput);
            
            if (pokemonDetails is null) return null;

            var response = new Pokemon();
            response.MapDetails(pokemonDetails);

            var speciesDetails = await _pokemonApiService.GetPokemonSpeciesDetails(sanitisedInput);

            if (speciesDetails != null)
            {
                response.MapSpecies(speciesDetails);
            }

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
