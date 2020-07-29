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
        private readonly ILogger<ApiHostController> _logger;
        private readonly IGameDataService _gameDataService;
        private readonly IPlayerAnswerService _playerAnswerService;
        private readonly IHttpContextAccessor _accessor;
        private readonly string cookieName = "gameName";
        public ApiHostController(ILogger<ApiHostController> logger,
            IHttpContextAccessor accessor,
            IGameDataService gameDataService,
            IPlayerAnswerService playerAnswerService)
        {
            _logger = logger;
            _accessor = accessor;
            _gameDataService = gameDataService;
            _playerAnswerService = playerAnswerService;
        }

        [HttpPost]
        [Route("/api/Host/CreateGame")]
        public async Task<IActionResult> CreateGame([FromBody] GameMetadata data)
        {
            var game = new Game();
            game.Metadata.Name = data.Name;
            var stored = await _gameDataService.SaveQuestions(game);
            return Ok(stored);
        }

        [HttpGet]
        [Route("/api/Host/GetQuestions")]
        public async Task<IActionResult> Questions([FromQuery] int? gameId)
        {
            if (gameId == null)
            {
                return BadRequest();
            }
            try
            {
                Game model;
                var found = await _gameDataService.LoadQuestions(gameId.Value);
                if (found == null)
                {
                    model = new Game();
                }
                else
                {
                    model = found;
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        [Route("/api/Host/SaveQuestions")]
        [HttpPost]
        public async Task<IActionResult> Questions([FromBody] Game game)
        {
            try
            {
                var gameMetadata = await _gameDataService.SaveQuestions(game);
                var options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(1),
                    Secure = true
                };

                HttpContext.Response.Cookies.Append(cookieName, gameMetadata.GameId.ToString(), options);
                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("/api/Host/GetPlayerAnswers")]
        public async Task<IActionResult> PlayerAnswers(int gameId)
        {
            var results = await _playerAnswerService.GetAllAnswers(gameId) ?? Array.Empty<PlayerAnswer>();
            return Ok(results);
        }

        [Route("/api/Host/SetCurrentQuestion")]
        [HttpPost]
        public async Task<IActionResult> SetCurrentQuestion([FromBody] GameMetadata game)
        {
            var result = await _gameDataService.SetCurrentQuestion(game);
            return Ok(result);
        }

        [Route("/api/Host/UpdatePlayerScores")]
        [HttpPost]
        public async Task<IActionResult> UpdatePlayerScores([FromBody] IEnumerable<PlayerAnswer> answers)
        {
            var submitted = await _playerAnswerService.SubmitPlayerScores(answers);
            return Ok(submitted);
        }

        [Route("/api/Host/GetScoredResponses")]
        [HttpGet]
        public async Task<IActionResult> GetScoredAnswers(int gameId)
        {
            var results = await _playerAnswerService.GetScoresForGame(gameId);
            return Ok(results);
        }

        [Route("/api/Host/ShowResults")]
        [HttpPost]
        public async Task<IActionResult> UpdateShowHideResults([FromBody] GameMetadata game)
        {
            var result = await _gameDataService.SetShowResults(game);
            return Ok(result);
        }

        [Route("/api/Host/SetState")]
        [HttpPost]
        public async Task<IActionResult> SetState([FromBody] GameMetadata game)
        {
            var response = await _gameDataService.SetGameState(game);
            return Ok(response);
        }

        [Route("/api/Host/DeletePlayerAnswer")]
        [HttpPost]
        public async Task<IActionResult> DeletePlayerAnswer([FromBody] PlayerAnswer answer)
        {
            var result = await _playerAnswerService.DeletePlayerAnswer(answer);
            return Ok(result);
        }

    }
}