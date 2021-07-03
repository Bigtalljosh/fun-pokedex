using FunPokedex.Business;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace FunPokedex.Api.IntegrationTests
{
    public class PokemonControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;
        private const string _base = "pokemon/";

        public PokemonControllerTests(WebApplicationFactory<Startup> factory)
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");

            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddJsonFile(configPath);
                });
            });
        }

        [Theory]
        [InlineData("mewtwo")]
        [InlineData("charmander")]
        [InlineData("94")]
        [InlineData("130")]
        [InlineData("mew")]
        public async Task Get_ShouldReturnPokemonObjectWithDetails_WhenPassedValidParam(string pokemonNameOrId)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"{_base}{pokemonNameOrId}");

            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseContent);
            var responseContentTyped = JsonSerializer.Deserialize<Pokemon>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(responseContentTyped.Name);
            Assert.NotNull(responseContentTyped.Description);
            Assert.NotNull(responseContentTyped.Habitat);
        }

        [Theory]
        [InlineData("Agumon")]
        [InlineData("Greymon")]
        [InlineData("99999")]
        [InlineData("&*(ds9s")]
        public async Task Get_ShouldReturnNotFound_WhenPassedNamesThatAreNotPokemonOrNumbersThatAreNotPokemon(string notPokemonNameOrId)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync($"{_base}{notPokemonNameOrId}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
