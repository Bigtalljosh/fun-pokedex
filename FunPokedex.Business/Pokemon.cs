using FunPokedex.PokemonApi;

namespace FunPokedex.Business
{
    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Habitat { get; set; }
        public bool IsLegendary { get; set; }
    }

    public static class PokemonExtensions
    {
        public static Pokemon MapDetails(this Pokemon pokemon, PokemonApiDetailsResponse pokemonApiResponse)
        {
            pokemon.Id = pokemonApiResponse.Id;
            pokemon.Name = pokemonApiResponse.Name;
            return pokemon;
        }

        public static Pokemon MapSpecies(this Pokemon pokemon, PokemonApiSpeciesResponse pokemonApiResponse)
        {
            pokemon.Description = pokemonApiResponse.FlavorTextEntries[0].FlavorText.Replace("\n", " ").Replace("\f", " "); // TODO make this cleaner, also need to make sure it's always English, Pikachu for example is Japanese which likely won't work in the other APIs
            pokemon.IsLegendary = pokemonApiResponse.IsLegendary;
            pokemon.Habitat = pokemonApiResponse.Habitat.Name;
            return pokemon;
        }
    }
}
