using FunPokedex.Business;
using System;
using System.Threading.Tasks;

namespace FunPokedex.Data
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly HttpClient
        public PokemonRepository()
        {

        }

        public Task<Pokemon> Get(string pokemonName)
        {
            throw new NotImplementedException();
        }
    }
}
