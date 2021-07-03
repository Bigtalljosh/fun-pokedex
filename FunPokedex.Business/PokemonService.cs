using FunPokedex.PokemonApi;
using FunPokedex.ShakespeareApi;
using FunPokemon.YodaApi;
using System;
using System.Threading.Tasks;

namespace FunPokedex.Business
{
    public class PokemonService : IPokemonService
    {
        private readonly IPokemonApiService _pokemonApiService;
        private readonly IYodaApiService _yodaApiService;
        private readonly IShakespeareApiService _shakespeareApiService;

        public PokemonService(IPokemonApiService pokemonApiService, IYodaApiService yodaApiService, IShakespeareApiService shakespeareApiService)
        {
            _pokemonApiService = pokemonApiService;
            _yodaApiService = yodaApiService;
            _shakespeareApiService = shakespeareApiService;
        }

        public async Task<Pokemon> Get(string pokemonNameOrId)
        {
            var sanitisedInput = SanitiseInput(pokemonNameOrId);
            var pokemonDetails = await _pokemonApiService.GetPokemonDetails(sanitisedInput);

            if (pokemonDetails is null)
            {
                return null;
            }

            return Pokemon.Map(pokemonDetails);
        }

        public async Task<Pokemon> GetTranslated(string pokemonNameOrId)
        {
            var pokemonResponse = await Get(pokemonNameOrId);

            try
            {
                if (pokemonResponse.IsLegendary || pokemonResponse.Habitat.Equals("cave"))
                {
                    var yodaResponse = await _yodaApiService.TranslateToYodaSpeak(pokemonResponse.Description);
                    if(yodaResponse != null && yodaResponse.Success.Total >= 1)
                    {
                        pokemonResponse.Description = yodaResponse.Contents.Translated;
                    }
                }
                else
                {
                    var shakespeareResponse = await _shakespeareApiService.TranslateToShakespeareSpeak(pokemonResponse.Description);
                    if (shakespeareResponse != null && shakespeareResponse.Success.Total >= 1)
                    {
                        pokemonResponse.Description = shakespeareResponse.Contents.Translated;
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
