using FunPokedex.PokemonApi;
using System;
using System.Linq;

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
                Description = pokemonApiResponse.FlavorTextEntries.FirstOrDefault(e => e.Language.Name.Equals("en", StringComparison.InvariantCultureIgnoreCase)).FlavorText.Replace("\n", " ").Replace("\f", " "),
                IsLegendary = pokemonApiResponse.IsLegendary,
                Habitat = pokemonApiResponse.Habitat.Name,
            };
        }
    }
}
