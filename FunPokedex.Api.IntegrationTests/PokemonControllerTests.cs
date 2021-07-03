using FunPokedex.Business;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.IO;
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
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{_base}{pokemonNameOrId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.NotNull(responseContent);

            var responseContentTyped = JsonSerializer.Deserialize<Pokemon>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.NotNull(responseContentTyped.Name);
            Assert.NotNull(responseContentTyped.Description);
            Assert.NotNull(responseContentTyped.Habitat);
        }
    }
}
