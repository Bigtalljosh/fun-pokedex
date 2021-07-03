using FunPokedex.PokemonApi;
using FunPokedex.ShakespeareApi;
using FunPokemon.YodaApi;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace FunPokedex.Business.UnitTests
{
    public class PokemonServiceTests
    {
        private readonly IPokemonApiService _pokemonApiService;
        private readonly IYodaApiService _yodaApiService;
        private readonly IShakespeareApiService _shakespeareApiService;

        public PokemonServiceTests()
        {
            var mockDetailsResponse = new PokemonApiResponse
            {
                Id = 94,
                Name = "Gengar",
                FlavorTextEntries = new System.Collections.Generic.List<FlavorTextEntry> {
                    new FlavorTextEntry { FlavorText = "Under a full moon, this POKÈMON likes to mimic the shadows of people and laugh at their fright." } },
                Habitat = new Habitat { Name = "cave" },
                IsLegendary = false
            };

            _pokemonApiService = Substitute.For<IPokemonApiService>();
            _pokemonApiService.GetPokemonDetails(Arg.Any<string>()).Returns(mockDetailsResponse);

            _yodaApiService = Substitute.For<IYodaApiService>();
            _shakespeareApiService = Substitute.For<IShakespeareApiService>();
        }

        [Fact]
        public async Task Get_ShouldReturnPokemonObjectWithDetails_WhenPassedValidParam()
        {
            var pokemonName = "gengar";
            var service = new PokemonService(_pokemonApiService, _yodaApiService, _shakespeareApiService);
           
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
            var service = new PokemonService(_pokemonApiService, _yodaApiService, _shakespeareApiService);

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
            PokemonApiResponse nullDetilsResponse = null;
            _pokemonApiService.GetPokemonDetails(Arg.Any<string>()).Returns(nullDetilsResponse);
            var service = new PokemonService(_pokemonApiService, _yodaApiService, _shakespeareApiService);

            var response = await service.Get(pokemonName);

            Assert.Null(response);
        }
    }
}
