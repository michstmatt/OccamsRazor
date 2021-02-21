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
    [Route("api/[controller]")]
    [ApiController]
    public class PlayController: Controller
    {
        private readonly ILogger<PlayController> _logger;
        private readonly IGameDataService _gameDataService;
        private readonly IPlayerAnswerService _playerAnswerService;
        private readonly INotificationService _notificationService;



        public PlayController(ILogger<PlayController> logger,
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
        [Route("LoadGames")]
        public async Task<IActionResult> Index()
        {
            var results = await _gameDataService.LoadGamesAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("GetCurrentQuestion")]
        public async Task<IActionResult> GetCurrentQuestion(int gameId, string host)
        {
            var question = await _gameDataService.GetCurrentQuestionAsync(gameId);
            var state = await _gameDataService.GetGameStateAsync(gameId);
            if (state.State != GameStateEnum.PostQuestion && (string.IsNullOrEmpty(host) || host != gameId.ToString()))
            {
                if(question is Question)
                    ((Question)(question)).AnswerText = "";
                else
                    ((MultipleChoiceQuestion)(question)).AnswerId = "";
            }
            return Ok(question);
        }
        [HttpGet]
        [Route("GetState")]
        public async Task<IActionResult> GetState(int gameId)
        {
            var gameState = await _gameDataService.GetGameStateAsync(gameId);
            return Ok(gameState);
        }

        [Route("SubmitAnswer")]
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

        [Route("GetScoredResponsesForPlayer")]
        [HttpGet]
        public async Task<IActionResult> GetScoredAnswersForPlayer(int gameId, string name)
        {
            var answers = await _playerAnswerService.GetScoresForPlayer(gameId, name);
            return Ok(answers);
        }

        [Route("GetScoredResponses")]
        [HttpGet]
        public async Task<IActionResult> GetScoredAnswers(int gameId)
        {
            var games = await _gameDataService.LoadGamesAsync();
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