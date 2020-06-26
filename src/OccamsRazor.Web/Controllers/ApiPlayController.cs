using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OccamsRazor.Web.Models;
using OccamsRazor.Web.Service;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Controllers
{
    public class ApiPlayController : Controller
    {
        private readonly ILogger<PlayController> _logger;
        private readonly IGameDataService _gameDataService;
        private readonly IPlayerAnswerService _playerAnswerService;

        private readonly IHttpContextAccessor _accessor;

        private readonly string CookiePlayerName = "PlayerName";
        private readonly string CookieGameName = "GameName";

        public ApiPlayController(ILogger<PlayController> logger,
            IGameDataService gameDataService,
            IPlayerAnswerService answerService,
            IHttpContextAccessor accessor)
        {
            _logger = logger;
            _gameDataService = gameDataService;
            _playerAnswerService = answerService;
            _accessor = accessor;
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
        public async Task<IActionResult> GetCurrentQuestion(string gameId, string host)
        {
            //var playerName = _accessor.HttpContext.Request.Cookies[CookiePlayerName];
            //var gameName = _accessor.HttpContext.Request.Cookies[CookieGameName];
            var question = await _gameDataService.GetCurrentQuestion(gameId);
            if(string.IsNullOrEmpty(host) || host!=gameId)
            {
                question.AnswerText = "";
            }
            return Ok(question);
        }

        [Route("/api/Play/SubmitAnswer")]
        [HttpPost]
        public async Task<IActionResult> SubmitAnswer([FromBody] PlayerAnswer answer)
        {
            
            var playerName = _accessor.HttpContext.Request.Cookies[CookiePlayerName];
            var gameName = _accessor.HttpContext.Request.Cookies[CookieGameName];
            //answer.GameId = int.Parse(gameName);
            //answer.Player = new Player { Name = playerName };
            var result = await _playerAnswerService.SubmitPlayerAnswer(answer);
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
            var ok = games.Where(g => g.GameId == gameId).FirstOrDefault().ShowResults;
            if(!ok)
            {
                return Ok(Array.Empty<GameResults>());
            }
            var results = await _playerAnswerService.GetScoresForGame(gameId);
            return Ok(results);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}