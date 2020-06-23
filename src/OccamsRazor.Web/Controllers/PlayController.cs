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
    public class PlayController : Controller
    {
        private readonly ILogger<PlayController> _logger;
        private readonly IGameDataService _gameDataService;
        private readonly IPlayerAnswerService _playerAnswerService;

        private readonly IHttpContextAccessor _accessor;

        private readonly string CookiePlayerName = "PlayerName";
        private readonly string CookieGameName = "GameName";

        public PlayController(ILogger<PlayController> logger,
            IGameDataService gameDataService,
            IPlayerAnswerService answerService,
            IHttpContextAccessor accessor)
        {
            _logger = logger;
            _gameDataService = gameDataService;
            _playerAnswerService = answerService;
            _accessor = accessor;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string selectedGameId, Player player)
        {
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                Secure = true
            };

            HttpContext.Response.Cookies.Append(CookiePlayerName, player?.Name ?? "", options);
            HttpContext.Response.Cookies.Append(CookieGameName, selectedGameId, options);
            return RedirectToAction("Answer");
        }

        public async Task<IActionResult> Answer()
        {
            var playerName = _accessor.HttpContext.Request.Cookies[CookiePlayerName];
            var gameName = _accessor.HttpContext.Request.Cookies[CookieGameName];
            var question = await _gameDataService.GetCurrentQuestion(gameName);
            return View(question);
        }

        [HttpPost]
        public async Task<IActionResult> Answer(PlayerAnswer answer)
        {
            var playerName = _accessor.HttpContext.Request.Cookies[CookiePlayerName];
            var gameName = _accessor.HttpContext.Request.Cookies[CookieGameName];
            answer.GameId = int.Parse(gameName);
            answer.Player = new Player { Name = playerName };
            var result = await _playerAnswerService.SubmitPlayerAnswer(answer);
            return View(await _gameDataService.GetCurrentQuestion(gameName));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}