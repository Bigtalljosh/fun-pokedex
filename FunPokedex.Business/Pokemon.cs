using FunPokedex.PokemonApi;
using System;
using System.Linq;

namespace FunPokedex.Business
{
    public record Pokemon
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public string Description { get; set; }
        public string Habitat { get; init; }
        public bool IsLegendary { get; init; }

        public static Pokemon Map(PokemonApiResponse pokemonApiResponse)
        {
            return new Pokemon
            {
                Id = pokemonApiResponse.Id,
                Name = pokemonApiResponse.Name,
                Description = pokemonApiResponse.FlavorTextEntries?
                    .FirstOrDefault(e => e.Language.Name.Equals("en", StringComparison.InvariantCultureIgnoreCase)).FlavorText
                    .Replace("\n", " ")
                    .Replace("\f", " ") 
                    ?? string.Empty,
                IsLegendary = pokemonApiResponse.IsLegendary,
                Habitat = pokemonApiResponse.Habitat.Name,
            };
        }
    }
}
