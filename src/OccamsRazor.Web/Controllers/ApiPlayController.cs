using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


using OccamsRazor.Web.Service;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Controllers
{
    public class ApiPlayController : Controller
    {
        private readonly ILogger<ApiPlayController> _logger;
        private readonly IGameDataService _gameDataService;
        private readonly IPlayerAnswerService _playerAnswerService;
        private readonly INotificationService _notificationService;



        public ApiPlayController(ILogger<ApiPlayController> logger,
            IGameDataService gameDataService,
            IPlayerAnswerService answerService,
            INotificationService notificationSvc
        )
        {
            _logger = logger;
            _gameDataService = gameDataService;
            _playerAnswerService = answerService;
            _notificationService = notificationSvc;
        }

        [HttpGet]
        [Route("/api/Play/LoadGames")]
        public async Task<IActionResult> Index()
        {
            var results = await _gameDataService.LoadGames();
            return Ok(results);
        }

        [HttpGet]
        [Route("/api/Play/GetCurrentQuestion")]
        public async Task<IActionResult> GetCurrentQuestion(int gameId, string host)
        {
            var question = await _gameDataService.GetCurrentQuestion(gameId);
            if (string.IsNullOrEmpty(host) || host != gameId.ToString())
            {
                question.AnswerText = "";
            }
            return Ok(question);
        }

        [HttpGet]
        [Route("/api/Play/GetState")]
        public async Task<IActionResult> GetState(int gameId)
        {
            var gameState = await _gameDataService.GetGameState(gameId);
            return Ok(gameState);
        }

        [Route("/api/Play/SubmitAnswer")]
        [HttpPost]
        public async Task<IActionResult> SubmitAnswer([FromBody] PlayerAnswer answer)
        {
            if(answer == null)
            {
                return BadRequest();
            }
            var result = await _playerAnswerService.SubmitPlayerAnswer(answer);
            await _notificationService.UpdateHost("NEW_ANSWER");
            return Ok(result);
        }

        [Route("/api/Play/GetScoredResponsesForPlayer")]
        [HttpGet]
        public async Task<IActionResult> GetScoredAnswersForPlayer(int gameId, string name)
        {
            var answers = await _playerAnswerService.GetScoresForPlayer(gameId, name);
            return Ok(answers);
        }

        [Route("/api/Play/GetScoredResponses")]
        [HttpGet]
        public async Task<IActionResult> GetScoredAnswers(int gameId)
        {
            var games = await _gameDataService.LoadGames();
            var ok = games.Where(g => g.GameId == gameId).FirstOrDefault().State == GameStateEnum.Results;
            if (!ok)
            {
                return Ok(Array.Empty<GameResults>());
            }
            var results = await _playerAnswerService.GetScoresForGame(gameId);
            return Ok(results);
        }
    }
}