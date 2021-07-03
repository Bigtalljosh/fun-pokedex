using System.Threading.Tasks;

namespace FunPokedex.PokemonApi
{
    public interface IPokemonApiService
    {
        Task<PokemonApiDetailsResponse> GetPokemonDetails(string pokemonName);
        Task<PokemonApiSpeciesResponse> GetPokemonSpeciesDetails(string pokemonName);
    }
}