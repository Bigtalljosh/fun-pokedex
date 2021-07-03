﻿using System.Threading.Tasks;

namespace FunPokedex.Business
{
    public interface IPokemonService
    {
        Task<Pokemon> Get(string pokemonName);
        Task<Pokemon> GetTranslated(string pokemonNameOrId);
    }
}