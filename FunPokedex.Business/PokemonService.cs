using FunPokedex.PokemonApi;
using FunPokedex.ShakespearApi;
using FunPokemon.YodaApi;
using System;
using System.Threading.Tasks;

namespace FunPokedex.Business
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonApiService _pokemonApiService;
        private readonly IYodaApiService _yodaApiService;
        private readonly IShakespearApiService _shakespearApiService;

        public PokemonService(IPokemonApiService pokemonApiService, IYodaApiService yodaApiService, IShakespearApiService shakespearApiService)
        {
            _pokemonApiService = pokemonApiService;
            _yodaApiService = yodaApiService;
            _shakespearApiService = shakespearApiService;
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

        public async Task<Pokemon> GetTranslated(string pokemonNameOrId)
        {
            var pokemonResponse = await Get(pokemonNameOrId);

            try
            {
                if (pokemonResponse.IsLegendary || pokemonResponse.Habitat.Equals("cave"))
                {
                    // Apply Yoda
                    var yodaResponse = await _yodaApiService.TranslateToYodaSpeak(pokemonResponse.Description);
                    if(yodaResponse != null && yodaResponse.Success.Total >= 1)
                    {
                        pokemonResponse.Description = yodaResponse.Contents.Translation;
                    }
                }
                else
                {
                    // Apply Shakespear
                    var shakespearResponse = await _shakespearApiService.TranslateToShakespearSpeak(pokemonResponse.Description);
                    if (shakespearResponse != null && shakespearResponse.Success.Total >= 1)
                    {
                        pokemonResponse.Description = shakespearResponse.Contents.Translation;
                    }
                }
            }
            catch(Exception)
            {
                // If we can't translate for any reason keep the normal description and return
                return pokemonResponse;
            }

            return pokemonResponse;
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
