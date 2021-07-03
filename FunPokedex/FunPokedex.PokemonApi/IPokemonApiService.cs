using System.Threading.Tasks;

namespace FunPokedex.PokemonApi
{
    public interface IPokemonApiService
    {
        Task<PokemonApiResponse> GetPokemonDetails(string pokemonName);
    }
}