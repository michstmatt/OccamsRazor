using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
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
    public class AuthenticationController : ControllerBase
    {

        private const string CookieHeader = "player";
        private readonly ILogger<HostController> _logger;
        private readonly IAuthenticationService _authService;
        private readonly IGameDataService _gameDataService;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _accessor;
        public AuthenticationController(ILogger<HostController> logger,
            IHttpContextAccessor accessor,
            IAuthenticationService authService,
            IGameDataService gameDataService,
            INotificationService notificationService)
        {
            _logger = logger;
            _accessor = accessor;
            _authService = authService;
            _gameDataService = gameDataService;
            _notificationService = notificationService;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateSession(int gameId, [FromBody] Player player)
        {
            player.Id = Guid.NewGuid();
            var token = await _authService.GenerateJwtToken(gameId, player.Id.ToString());
            if(token == null)
                return BadRequest();
            return Ok(token);
        }

    }
}