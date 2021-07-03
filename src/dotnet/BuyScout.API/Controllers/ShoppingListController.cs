using System;
using System.Collections.Generic;
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

            await _hubContext.Clients.All.SendCoreAsync("Broadcast", new[]
            {
                "ShoppingList",
                $"Called Get at {DateTime.Now}"
            });

            var result = await _repository.QueryAsync<ShoppingList>(x => true);
            return result;
        }

        [HttpPost("{listId}")]
        public async Task<IActionResult> AddItem(
            [FromRoute] string listId,
            [FromBody] string title)
        {
            await _hubContext.Clients.All.SendCoreAsync("AddItem", new[]
            {
                listId,
                title,
                $"Called Get at {DateTime.Now}"
            });
            return Ok();
        }
    }
}
