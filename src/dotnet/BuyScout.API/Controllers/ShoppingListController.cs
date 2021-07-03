using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuyScout.Domain.Interfaces;
using BuyScout.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BuyScout.API.Controllers
{ 
    [ApiController]
    [Route("[controller]")]
    public class ShoppingListController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IHubContext<TestHub> _hubContext;
        private readonly ILogger<ShoppingListController> _logger; 

        public ShoppingListController(
            IRepository repository,
            IHubContext<TestHub> hubContext,
            ILogger<ShoppingListController> logger)
        {
            _repository = repository;
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ShoppingList>> Get()
        {
            _logger.LogInformation("Called Get at {Timestamp}", DateTime.Now);

            await _hubContext.Clients.All.SendCoreAsync("Broadcast", new []
            {
                "WeatherForecast",
                $"Called Get at {DateTime.Now}"
            });

            var result = await _repository.QueryAsync<ShoppingList>(x => true);
            return result;
        }
    }
}
