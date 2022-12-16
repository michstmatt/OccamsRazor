namespace OccamsRazor.Controllers
{
    using OccamsRazor.Models;
    using OccamsRazor.Services;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController: ControllerBase
    {
        private readonly ILogger<AuthenticationController> logger;
        private readonly TokenService tokenService;

        public AuthenticationController(ILogger<AuthenticationController> logger, TokenService tokenService)
        {
            this.logger = logger;
            this.tokenService = tokenService;
        }

        [HttpGet]
        [Route("Join")]
        public IActionResult Join([FromQuery] string gameId, [FromQuery] string playerName, [FromQuery] string gameKey)
        {
            var player = new Player(playerName, gameId);
            var jwtToken = tokenService.GenerateToken(player);

            return Ok(jwtToken);
        }
    }
}