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
    [Route("api/[controller]")]
    [ApiController]
    public class HostController: Controller
    {
        private readonly ILogger<HostController> _logger;
        private readonly IAuthenticationService _authService;
        private readonly IGameDataService _gameDataService;
        private readonly IPlayerAnswerService _playerAnswerService;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _accessor;
        public HostController(ILogger<HostController> logger,
            IHttpContextAccessor accessor,
            IAuthenticationService authService,
            IGameDataService gameDataService,
            IPlayerAnswerService playerAnswerService,
            INotificationService notificationService)
        {
            _logger = logger;
            _accessor = accessor;
            _authService = authService;
            _gameDataService = gameDataService;
            _playerAnswerService = playerAnswerService;
            _notificationService = notificationService;
        }

        [HttpGet]
        [Route("Validate")]
        public async Task<IActionResult> Validate(int gameId, [FromHeader(Name="gameKey")]string gameKey)
        {
            if (await _authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }

            return Ok();
        }

        [HttpPost]
        [Route("CreateGame")]
        public async Task<IActionResult> CreateGame([FromBody] GameMetadata data, [FromHeader(Name="gameKey")] string gameKey)
        {
            var game = new Game();
            game.Metadata.Name = data.Name;
            var stored = await _gameDataService.CreateGameAsync(game);
            await _authService.AddAuthentication(stored.GameId, gameKey);
            return Ok(stored);
        }



        [HttpGet]
        [Route("GetQuestions")]
        public async Task<IActionResult> Questions([FromQuery] int? gameId, [FromHeader(Name="gameKey")] string gameKey)
        {
            
            if (await _authService.IsAuthenticated(gameId?? 0, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            if (gameId == null)
            {
                return BadRequest();
            }
            try
            {
                var found = await _gameDataService.LoadGameAsync(gameId.Value);
                if (found == null)
                {
                    return BadRequest();
                }

                return Ok(found);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [Route("SaveQuestions")]
        [HttpPost]
        public async Task<IActionResult> Questions([FromBody] Game game, [FromHeader(Name="gameKey")]string gameKey)
        {
            if (await _authService.IsAuthenticated(game.Metadata.GameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            try
            {
                var gameMetadata = await _gameDataService.SaveGameAsync(game);
                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetPlayerAnswers")]
        public async Task<IActionResult> PlayerAnswers(int gameId, [FromHeader(Name="gameKey")]string gameKey)
        {
            if (await _authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }

            var results = await _playerAnswerService.GetAllAnswers(gameId) ?? Array.Empty<PlayerAnswer>();
            return Ok(results);
        }

        [Route("SetCurrentQuestion")]
        [HttpPost]
        public async Task<IActionResult> SetCurrentQuestion([FromBody] GameMetadata game, [FromHeader(Name="gameKey")] string gameKey)
        {
            if (await _authService.IsAuthenticated(game.GameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            game.State = GameStateEnum.PreQuestion;
            await _gameDataService.SetGameStateAsync(game);
            var result = await _gameDataService.SetCurrentQuestionAsync(game);

            await _notificationService.SendPlayerMessage("NEW_QUESTION");
            return Ok(result);
        }

        [Route("UpdatePlayerScores")]
        [HttpPost]
        public async Task<IActionResult> UpdatePlayerScores(int gameId, [FromBody] IEnumerable<PlayerAnswer> answers, [FromHeader(Name="gameKey")] string gameKey)
        {
            if (await _authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            var submitted = await _playerAnswerService.SubmitPlayerScores(answers);
            return Ok(submitted);
        }

        [Route("GetScoredResponses")]
        [HttpGet]
        public async Task<IActionResult> GetScoredAnswers(int gameId, [FromHeader(Name="gameKey")] string gameKey)
        {
            var game = await _gameDataService.GetGameStateAsync(gameId);
            if (game.State != GameStateEnum.Results && await _authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            var results = await _playerAnswerService.GetScoresForGame(gameId);
            return Ok(results);
        }

        [Route("SetState")]
        [HttpPost]
        public async Task<IActionResult> SetState([FromBody] GameMetadata game, [FromHeader(Name="gameKey")] string gameKey)
        {
            if (await _authService.IsAuthenticated(game.GameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            var response = await _gameDataService.SetGameStateAsync(game);
            await _notificationService.SendPlayerMessage("STATE_CHANGED");
            return Ok(response);
        }

        [Route("DeletePlayerAnswer")]
        [HttpDelete]
        public async Task<IActionResult> DeletePlayerAnswer(int gameId, [FromBody] PlayerAnswer answer, [FromHeader(Name="gameKey")] string gameKey)
        {
            if (await _authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            var result = await _playerAnswerService.DeletePlayerAnswer(answer);
            return Ok(result);
        }

        [Route("DeleteGame")]
        [HttpDelete]
        public async Task<IActionResult> DeleteGame(int gameId, [FromHeader(Name="gameKey")] string gameKey)
        {
            if (await _authService.IsAuthenticated(gameId, gameKey ?? "") == false)
            {
                return Unauthorized();
            }
            var result = await _gameDataService.DeleteGameAsync(gameId);
            return Ok(result);
        }

        [HttpPost]
        [Route("CreateMcGame")]
        public async Task<IActionResult> CreateMcGame([FromBody] GameMetadata data)
        {
            var game = new Game();
            game.Metadata.Name = data.Name;
            game.Metadata.Seed = new Random().Next();
            game.Metadata.IsMultipleChoice = true;
            var stored = await _gameDataService.CreateGameAsync(game);
            return Ok(stored);
        }

    }
}