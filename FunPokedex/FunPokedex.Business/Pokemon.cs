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

        public static Pokemon Map(PokemonApiResponse pokemonApiResponse)
        {
            return new Pokemon
            {
                Id = pokemonApiResponse.Id,
                Name = pokemonApiResponse.Name,
                Description = string.Empty, // Get from different call
                IsLegendary = false // Get from different call
            };
        }
    }
}
