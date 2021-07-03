using FunPokedex.PokemonApi;
using FunPokedex.PokemonApiUnitTests.Mocks;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace FunPokedex.PokemonApiUnitTests
{
    public class PokemonApiServiceTests
    {
        [Fact]
        public async Task GetPokemonDetails_ShouldReturnPokemonDetailsObject_WhenPassedValidQueryParam()
        {
            var pokemonName = "mewtwo";
            var mockedResponseJson = File.ReadAllText("MockedPokemonDetailsResponse.json");
            var mockHttpHandler = new MockHttpMessageHandler(mockedResponseJson, HttpStatusCode.OK);
            var httpClient = new HttpClient(mockHttpHandler)
            {
                BaseAddress = new Uri("https://testbase.com/")
            };

            var service = new PokemonApiService(httpClient);
            var pokemon = await service.GetPokemonDetails(pokemonName);

            Assert.NotNull(pokemon);
        }

        [Fact]
        public async Task GetPokemonDetails_ShouldReturnNull_WhenPassedInValidQueryParam()
        {
            var pokemonName = "mewtwo";
            var mockHttpHandler = new MockHttpMessageHandler(string.Empty, HttpStatusCode.NotFound);
            var httpClient = new HttpClient(mockHttpHandler)
            {
                BaseAddress = new Uri("https://testbase.com/")
            };

            var service = new PokemonApiService(httpClient);
            var pokemon = await service.GetPokemonDetails(pokemonName);

            Assert.Null(pokemon);
        }

        [Fact]
        public async Task GetPokemonSpeciesDetails_ShouldReturnPokemonSpeciesDetailsObject_WhenPassedValidQueryParam()
        {
            var pokemonName = "mewtwo";
            var mockedResponseJson = File.ReadAllText("MockedPokemonSpeciesResponse.json");
            var mockHttpHandler = new MockHttpMessageHandler(mockedResponseJson, HttpStatusCode.OK);
            var httpClient = new HttpClient(mockHttpHandler)
            {
                BaseAddress = new Uri("https://testbase.com/")
            };

            var service = new PokemonApiService(httpClient);
            var pokemon = await service.GetPokemonSpeciesDetails(pokemonName);

            Assert.NotNull(pokemon);
        }

        [Fact]
        public async Task GetPokemonSpeciesDetails_ShouldReturnNull_WhenPassedInValidQueryParam()
        {
            var pokemonName = "mewtwo";
            var mockHttpHandler = new MockHttpMessageHandler(string.Empty, HttpStatusCode.NotFound);
            var httpClient = new HttpClient(mockHttpHandler)
            {
                BaseAddress = new Uri("https://testbase.com/")
            };

            var service = new PokemonApiService(httpClient);
            var pokemon = await service.GetPokemonSpeciesDetails(pokemonName);

            Assert.Null(pokemon);
        }
    }
}
