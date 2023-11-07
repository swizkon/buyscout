using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuyScout.Contracts;
using BuyScout.Domain.Interfaces;
using BuyScout.Domain.Model;
using MassTransit;
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
        private readonly IBus _bus;
        private readonly ILogger<ShoppingListController> _logger; 

        public ShoppingListController(
            IRepository repository,
            IHubContext<TestHub> hubContext,
            IBus bus,
            ILogger<ShoppingListController> logger)
        {
            _repository = repository;
            _hubContext = hubContext;
            _bus = bus;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ShoppingList>> Get()
        {
            _logger.LogInformation("Called Get at {Timestamp}", DateTime.Now);

            await _hubContext.Clients.All.SendCoreAsync("Broadcast", new[]
            {
                "ShoppingList",
                $"Called at {DateTime.Now}"
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
                Guid.NewGuid().ToString(),
                $"Called at {DateTime.Now}"
            });
            
            await _bus.Publish(new AddItemToListCommand
            {
                ListId = listId,
                Title = title,
                Description = "Description"
            });

            return Ok();
        }

        [HttpDelete("{listId}/{itemId}/{reason}")]
        public async Task<IActionResult> RemoveItem(
            [FromRoute] string listId,
            [FromRoute] string itemId,
            [FromRoute] string reason)
        {
            await _hubContext.Clients.All.SendCoreAsync("RemoveItem", new[]
            {
                listId,
                itemId,
                reason,
                $"Called at {DateTime.Now}"
            });
            return Ok();
        }
    }
}
