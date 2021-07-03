using FunPokedex.PokemonApi;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunPokedex.Business
{
    public class PokemonService : IPokemonService
    {
        private readonly ILogger _logger;
        private readonly IPokemonApiService _pokemonApiService;

        public PokemonService(ILogger<PokemonService> logger, IPokemonApiService pokemonApiService)
        {
            _logger = logger;
            _pokemonApiService = pokemonApiService;
        }

        public async Task<Pokemon> Get(string pokemonName)
        {
            var pokemonDetails = PokemonExtensions.MapDetails(await _pokemonApiService.GetPokemonDetails(pokemonName));
            pokemonDetails.MapSpecies(await _pokemonApiService.GetPokemonSpeciesDetails(pokemonName));
            return pokemonDetails;
        }
    }
}
