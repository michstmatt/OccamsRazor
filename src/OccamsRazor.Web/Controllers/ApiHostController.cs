using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Models;
using OccamsRazor.Web.Service;

namespace OccamsRazor.Web.Controllers
{
    public class ApiHostController : Controller
    {
        private readonly ILogger<HostController> _logger;
        private readonly IGameDataService _gameDataService;
        private readonly IPlayerAnswerService _playerAnswerService;
        private readonly IHttpContextAccessor _accessor;
        private readonly string cookieName = "gameName";
        public ApiHostController(ILogger<HostController> logger,
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
        public async Task<IActionResult> CreateGame([FromBody]GameMetadata data)
        {
            var game = new Game();
            game.Metadata.Name = data.Name;
            var stored = await _gameDataService.SaveQuestions(game);
            return Ok(stored);
        }

        [HttpGet]
        [Route("/api/Host/GetQuestions")]
        public async Task<IActionResult> Questions(string id)
        {
            Game model;
            var found = await _gameDataService.LoadQuestions(id);
            if (found == null)
                model = new Game();
            else
                model = found;

            //model.Metadata.Name = "test";
            return Ok(model);
        }

        [Route("/api/Host/SaveQuestions")]
        [HttpPost]
        public async Task<IActionResult> Questions([FromBody] Game game)
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

        [HttpGet]
        [Route("/api/Host/GetPlayerAnswers")]
        public async Task<IActionResult> PlayerAnswers(int id)
        {
            var results = await _playerAnswerService.GetAllAnswers(id);
            return Ok(results);
        }

        [Route("/api/Host/SetCurrentQuestion")]
        [HttpPost]
        public async Task<IActionResult> SetCurrentQuestion([FromBody]GameMetadata game)
        {
            var result = await _gameDataService.SetCurrentQuestion(game);
            return Ok(result);
        }

        [Route("/api/Host/UpdatePlayerScores")]
        [HttpPost]
        public async Task<IActionResult> UpdatePlayerScores([FromBody]IEnumerable<PlayerAnswer> answers)
        {
            var ok = await _playerAnswerService.SubmitPlayerScores(answers); 
            //var result = await _playerAnswerService.GetAllAnswers(answers[0])
            return Ok(true);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}