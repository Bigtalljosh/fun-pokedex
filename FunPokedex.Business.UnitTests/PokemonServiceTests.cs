using FunPokedex.PokemonApi;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace FunPokedex.Business.UnitTests
{
    public class PokemonServiceTests
    {
        private readonly IPokemonApiService _pokemonApiService;

        public PokemonServiceTests()
        {
            var mockDetailsResponse = new PokemonApiDetailsResponse { Id = 94, Name = "Gengar" };
            var mockSpeciesResponse = new PokemonApiSpeciesResponse
            {
                FlavorTextEntries = new System.Collections.Generic.List<FlavorTextEntry> {
                    new FlavorTextEntry { FlavorText = "Under a full moon, this POKÈMON likes to mimic the shadows of people and laugh at their fright." } },
                Habitat = new Habitat { Name = "cave" },
                IsLegendary = false
            };

            _pokemonApiService = Substitute.For<IPokemonApiService>();
            _pokemonApiService.GetPokemonDetails(Arg.Any<string>()).Returns(mockDetailsResponse);
            _pokemonApiService.GetPokemonSpeciesDetails(Arg.Any<string>()).Returns(mockSpeciesResponse);
        }

        [Fact]
        public async Task Get_ShouldReturnPokemonObjectWithDetails_WhenPassedValidParam()
        {
            var pokemonName = "gengar";
            var service = new PokemonService(_pokemonApiService);
           
            var response = await service.Get(pokemonName);

            Assert.NotNull(response);
            Assert.Equal(94, response.Id);
            Assert.Equal("Gengar", response.Name);
            Assert.Equal("Under a full moon, this POKÈMON likes to mimic the shadows of people and laugh at their fright.", response.Description);
            Assert.Equal("cave", response.Habitat);
            Assert.False(response.IsLegendary);
        }

        [Theory]
        [InlineData("gen gar")]
        [InlineData("gen.gar")]
        [InlineData("gengar'")]
        [InlineData("GENGAR")]
        public async Task Get_ShouldReturnPokemonObjectWithDetails_WhenPassedANameSpacesApostropheFullStopOrUppercase(string pokemonName)
        {
            var service = new PokemonService(_pokemonApiService);

            var response = await service.Get(pokemonName);

            Assert.NotNull(response);
            Assert.Equal(94, response.Id);
            Assert.Equal("Gengar", response.Name);
            Assert.Equal("Under a full moon, this POKÈMON likes to mimic the shadows of people and laugh at their fright.", response.Description);
            Assert.Equal("cave", response.Habitat);
            Assert.False(response.IsLegendary);
        }

        [Fact]
        public async Task Get_ShouldReturnNull_WhenApiResponseNull()
        {
            var pokemonName = "gengar";
            PokemonApiDetailsResponse nullDetilsResponse = null;
            PokemonApiSpeciesResponse nullSpeciesResponse = null;
            _pokemonApiService.GetPokemonDetails(Arg.Any<string>()).Returns(nullDetilsResponse);
            _pokemonApiService.GetPokemonSpeciesDetails(Arg.Any<string>()).Returns(nullSpeciesResponse);
            var service = new PokemonService(_pokemonApiService);

            var response = await service.Get(pokemonName);

            Assert.Null(response);
        }

        [Fact]
        public async Task Get_ShouldReturnPartialContent_WhenSpeciesApiResponseNull()
        {
            var pokemonName = "gengar";
            PokemonApiSpeciesResponse nullSpeciesResponse = null;
            _pokemonApiService.GetPokemonSpeciesDetails(Arg.Any<string>()).Returns(nullSpeciesResponse);
            var service = new PokemonService(_pokemonApiService);

            var response = await service.Get(pokemonName);

            Assert.NotNull(response);
            Assert.Equal(94, response.Id);
            Assert.Equal("Gengar", response.Name);
        }
    }
}
