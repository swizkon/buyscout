using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BuyScout.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IHubContext<TestHub> _hubContext;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(
            IHubContext<TestHub> hubContext,
            ILogger<WeatherForecastController> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogInformation("Called Get at {Timestamp}", DateTime.Now);

            await _hubContext.Clients.All.SendCoreAsync("Broadcast", new []
            {
                "WeatherForecast",
                $"Called Get at {DateTime.Now}"
            });

            await _hubContext.Clients.All.SendCoreAsync("WeatherForecastRequested", new []
            {
                "WeatherForecast",
                $"Called Get at {DateTime.Now}"
            });

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
