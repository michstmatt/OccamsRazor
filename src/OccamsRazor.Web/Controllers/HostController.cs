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
    public class HostController : Controller
    {
        private readonly ILogger<HostController> _logger;
        private readonly IGameDataService _gameDataService;
        private readonly IPlayerAnswerService _playerAnswerService;
        private readonly IHttpContextAccessor _accessor;
        private readonly string cookieName = "gameName";
        public HostController(ILogger<HostController> logger,
            IHttpContextAccessor accessor,
            IGameDataService gameDataService,
            IPlayerAnswerService playerAnswerService)
        {
            _logger = logger;
            _accessor = accessor;
            _gameDataService = gameDataService;
            _playerAnswerService = playerAnswerService;
        }

        public IActionResult Index(string key)
        {
            if (key == Environment.GetEnvironmentVariable("HOST_KEY"))
                return View();
            return Redirect("/");
        }

        public IActionResult Questions()
        {
            return View();
        }

/*
        [HttpPost]
        public async Task<IActionResult> Questions(Game game)
        {
            /*
            var gameMetadata = await _gameDataService.SaveQuestions(game);
            var options = new CookieOptions
            {
                Expires = DateTime.Now.AddDays(1),
                Secure = true
            };

            HttpContext.Response.Cookies.Append(cookieName, gameMetadata.GameId.ToString(), options);
            return await Questions();
            
        }
        */

        public async Task<IActionResult> Score()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
