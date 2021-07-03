﻿using FunPokedex.PokemonApi;

namespace FunPokedex.Business
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }

        public static Pokemon Map(PokemonApiResponse pokemonApiResponse)
        {
            return new Pokemon
            {
                Id = pokemonApiResponse.Id,
                Name = pokemonApiResponse.Name,
                Description = pokemonApiResponse.FlavorTextEntries[0].FlavorText.Replace("\n", " ").Replace("\f", " "), // TODO make this cleaner, also need to make sure it's always English, Pikachu for example is Japanese which likely won't work in the other APIs
                IsLegendary = pokemonApiResponse.IsLegendary,
                Habitat = pokemonApiResponse.Habitat.Name,
            };
        }
    }
}
