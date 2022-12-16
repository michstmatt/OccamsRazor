namespace OccamsRazor.Controllers
{
    using OccamsRazor.Models;
    using OccamsRazor.Services;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.AspNetCore.Authorization;

    [Route("game/[controller]")]
    [ApiController]
    public class PlayerController: ControllerBase
    {
        private readonly ILogger<PlayerController> logger;
        private readonly GameService gameService;
        private readonly TokenService tokenService;

        public PlayerController(ILogger<PlayerController> logger, GameService gameService, TokenService tokenService)
        {
            this.logger = logger;
            this.gameService = gameService;
            this.tokenService = tokenService;
        }

        [Route("join")]
        public async Task<IActionResult> JoinWs([FromQuery] string authorization)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest == false)
            {
                return BadRequest("Expected a websocket connection");
            }

            var player = tokenService.ValidateToken(authorization);
            using var websocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await gameService.OnPlayerConnected(player, websocket);
            return Ok();
        }
    }
}