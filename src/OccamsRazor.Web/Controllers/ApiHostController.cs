using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Service;

namespace OccamsRazor.Web.Controllers
{
    public class ApiHostController : Controller
    {
        private readonly ILogger<ApiHostController> logger;
        private readonly IAuthenticationService authService;
        private readonly IGameDataService gameDataService;
        private readonly IPlayerAnswerService playerAnswerService;
        public ApiHostController(ILogger<ApiHostController> logger,
            IAuthenticationService authService,
            IGameDataService gameDataService,
            IPlayerAnswerService playerAnswerService)
        {
            this.logger = logger;
            this.authService = authService;
            this.gameDataService = gameDataService;
            this.playerAnswerService = playerAnswerService;
        }

        [HttpGet]
        [Route("/api/Host/Validate")]
        public async Task<IActionResult> Validate(int gameId, [FromHeader(Name = "gameKey")] string gameKey)
        {
            if (await authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }

            return Ok();
        }

        [HttpPost]
        [Route("/api/Host/CreateGame")]
        public async Task<IActionResult> CreateGame([FromBody] GameMetadata data, [FromHeader(Name = "gameKey")] string gameKey)
        {
            var game = new Game();
            game.Metadata.Name = data.Name;
            var stored = await gameDataService.SetQuestionsAsync(game);
            await authService.AddAuthentication(stored.GameId, gameKey);
            return Ok(stored);
        }

        [HttpGet]
        [Route("/api/Host/GetQuestions")]
        public async Task<IActionResult> Questions([FromQuery] int? gameId, [FromHeader(Name = "gameKey")] string gameKey)
        {
            if (await authService.IsAuthenticated(gameId ?? 0, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            if (gameId == null)
            {
                return BadRequest();
            }
            try
            {
                var found = await gameDataService.GetQuestionsAsync(gameId.Value);
                if (found == null)
                {
                    return BadRequest();
                }

                return Ok(found);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [Route("/api/Host/SaveQuestions")]
        [HttpPost]
        public async Task<IActionResult> Questions([FromBody] Game game, [FromHeader(Name = "gameKey")] string gameKey)
        {
            if (await authService.IsAuthenticated(game.Metadata.GameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            try
            {
                var gameMetadata = await gameDataService.SetQuestionsAsync(game);
                return Ok(true);
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/api/Host/GetPlayerAnswers")]
        public async Task<IActionResult> PlayerAnswers(int gameId, [FromHeader(Name = "gameKey")] string gameKey)
        {
            if (await authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }

            var results = await playerAnswerService.GetAllAnswers(gameId) ?? Array.Empty<PlayerAnswer>();
            return Ok(results);
        }

        [Route("/api/Host/SetCurrentQuestion")]
        [HttpPost]
        public async Task<IActionResult> SetCurrentQuestion([FromBody] GameMetadata game, [FromHeader(Name = "gameKey")] string gameKey)
        {
            if (await authService.IsAuthenticated(game.GameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            var result = await gameDataService.SetCurrentQuestionAsync(game);
            return Ok(result);
        }

        [Route("/api/Host/UpdatePlayerScores")]
        [HttpPost]
        public async Task<IActionResult> UpdatePlayerScores(int gameId, [FromBody] IEnumerable<PlayerAnswer> answers, [FromHeader(Name = "gameKey")] string gameKey)
        {
            if (await authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            var submitted = await playerAnswerService.SubmitPlayerScores(answers);
            return Ok(submitted);
        }

        [Route("/api/Host/GetScoredResponses")]
        [HttpGet]
        public async Task<IActionResult> GetScoredAnswers(int gameId, [FromHeader(Name = "gameKey")] string gameKey)
        {
            var game = await gameDataService.GetGameStateAsync(gameId);
            if (game.State != GameStateEnum.Results && await authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            var results = await playerAnswerService.GetScoresForGame(gameId);
            return Ok(results);
        }

        [Route("/api/Host/SetState")]
        [HttpPost]
        public async Task<IActionResult> SetState([FromBody] GameMetadata game, [FromHeader(Name = "gameKey")] string gameKey)
        {
            if (await authService.IsAuthenticated(game.GameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            var response = await gameDataService.SetGameStateAsync(game);
            return Ok(response);
        }

        [Route("/api/Host/DeletePlayerAnswer")]
        [HttpDelete]
        public async Task<IActionResult> DeletePlayerAnswer(int gameId, [FromBody] PlayerAnswer answer, [FromHeader(Name = "gameKey")] string gameKey)
        {
            if (await authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            var result = await playerAnswerService.DeletePlayerAnswer(answer);
            return Ok(result);
        }

        [Route("/api/Host/DeleteGame")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGame(int gameId, [FromHeader(Name = "gameKey")] string gameKey)
        {
            if (await authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            var result = await gameDataService.DeleteGameAsync(gameId);
            return Ok(result);
        }

    }
}