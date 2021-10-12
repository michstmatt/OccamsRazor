using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using OccamsRazor.Common.Models;
using OccamsRazor.Web.Service;

namespace OccamsRazor.Web.Controllers
{
    [Route("health")]
    [ApiController]
    public class IndexController: Controller
    {
        private readonly ILogger<IndexController> _logger;

        public IndexController(ILogger<IndexController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index() 
        {
            return Ok(System.Environment.GetEnvironmentVariable("CONNECTION_STRING"));
            //return Ok(DateTime.UtcNow);
        }

    }
}