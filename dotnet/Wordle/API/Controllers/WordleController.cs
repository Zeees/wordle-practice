using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Wordle.Models.Domain;
using Wordle.Services;

namespace WordleAPI.Controllers
{
    [SwaggerTag("Start, stop and play wordle games.")]
    [Route("api/[controller]")]
    [ApiController]
    public class WordleController : ControllerBase
    {

        private readonly IWordleService _wordleService;

        public WordleController(
            IWordleService wordleService)
        {
            _wordleService = wordleService;
        }

        /// <summary>
        ///     Initilize a game session.
        /// </summary>
        /// <response code="200">The game session was created.</response>
        /// <response code="500">Something went wrong during the request, please try again later.</response>
        [HttpPost("start")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult<InitialGameResponse>> InitiateGameAsync([FromBody]InitiateGameRequest request)
        {
            var response = await _wordleService.InitilizeGameAsync(request);
            return Ok(response);
        }

        /// <summary>
        ///     Perform a guess in a Wordle game.
        /// </summary>
        /// <response code="200">A guess was made successfully.</response>
        /// <response code="404">Couldn't find the given game session.</response>
        /// <response code="500">Something went wrong with the request, try again and if that dosen't work contact support.</response>
        [HttpPost("guess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        [Produces("application/json")]
        public async Task<ActionResult<GuessResponse>> GuessAsync(GuessRequest request)
        {
            var response = await _wordleService.PerformGuessAsync(request);
            if(response == null) { return NotFound(); }
            return Ok(response);
        }


        /// <summary>
        ///     Aborts the wordle game session that belongs to the given ID.
        /// </summary>
        /// <response code="204">Game aborted, returns nothing.</response>
        /// <response code="404">There's no game session with the given id.</response>
        /// <response code="500">Something went wrong with the request, try again and if that dosen't work contact support.</response>
        /// <remarks>
        ///     Example request:
        ///     
        ///         DELETE Wordle/abort/1e80a24f-1399-43ea-ba0f-306f0a0dc0c6
        ///         
        /// </remarks>
        [HttpDelete("abort/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> AbortGameAsync(Guid id)
        {
            var resp = await _wordleService.AbortGameAsync(id);
            if(!resp) { return NotFound(); }
            return NoContent();
        }
    }
}
