using FunPokedex.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FunPokedex.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly ILogger<PokemonController> _logger;
        private readonly IPokemonService _pokemonService;

        public PokemonController(ILogger<PokemonController> logger, IPokemonService pokemonService)
        {
            _logger = logger;
            _pokemonService = pokemonService;
        }

        [HttpGet]
        [Route("{pokemonName}")]
        public async Task<IActionResult> Get(string pokemonName)
        {
            var pokemon = await _pokemonService.Get(pokemonName);

            if (pokemon != null)
            {
                return Ok(pokemon);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/translated/{pokemonName}")]
        public async Task<IActionResult> GetTranslated(string pokemonName)
        {
            var pokemon = await _pokemonService.GetTranslated(pokemonName);

            if (pokemon != null)
            {
                return Ok(pokemon);
            }

            return NotFound();
        }
    }
}
