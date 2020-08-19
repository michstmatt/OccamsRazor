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
        private readonly ILogger<ApiPlayController> logger;
        private readonly IGameDataService gameDataService;
        private readonly IPlayerAnswerService playerAnswerService;



        public ApiPlayController(ILogger<ApiPlayController> logger,
            IGameDataService gameDataService,
            IPlayerAnswerService answerService
        )
        {
            this.logger = logger;
            this.gameDataService = gameDataService;
            this.playerAnswerService = answerService;
        }

        [HttpGet]
        [Route("/api/Play/LoadGames")]
        public async Task<IActionResult> Index()
        {
            var results = await gameDataService.GetGamesAsync();
            return Ok(results);
        }

        [HttpGet]
        [Route("/api/Play/GetCurrentQuestion")]
        public async Task<IActionResult> GetCurrentQuestion(int gameId, string host)
        {
            var question = await gameDataService.GetCurrentQuestionAsync(gameId);
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
            var gameState = await gameDataService.GetGameStateAsync(gameId);
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
            var result = await playerAnswerService.SubmitPlayerAnswer(answer);
            return Ok(result);
        }

        [Route("/api/Play/GetScoredResponsesForPlayer")]
        [HttpGet]
        public async Task<IActionResult> GetScoredAnswersForPlayer(int gameId, string name)
        {
            var answers = await playerAnswerService.GetScoresForPlayer(gameId, name);
            return Ok(answers);
        }

        [Route("/api/Play/GetScoredResponses")]
        [HttpGet]
        public async Task<IActionResult> GetScoredAnswers(int gameId)
        {
            var game = await gameDataService.GetGameStateAsync(gameId);
            if (game.State != GameStateEnum.Results)
            {
                return Ok(Array.Empty<GameResults>());
            }
            var results = await playerAnswerService.GetScoresForGame(gameId);
            return Ok(results);
        }
    }
}