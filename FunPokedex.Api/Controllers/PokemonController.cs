using FunPokedex.Business;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FunPokedex.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        [HttpGet]
        [Route("{pokemonNameOrId}")]
        public async Task<IActionResult> Get(string pokemonNameOrId)
        {
            var pokemon = await _pokemonService.Get(pokemonNameOrId);

            if (pokemon is not null)
            {
                return Ok(pokemon);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("/translated/{pokemonNameOrId}")]
        public async Task<IActionResult> GetTranslated(string pokemonNameOrId)
        {
            var pokemon = await _pokemonService.GetTranslated(pokemonNameOrId);

            if (pokemon is not null)
            {
                return Ok(pokemon);
            }

            return NotFound();
        }
    }
}
