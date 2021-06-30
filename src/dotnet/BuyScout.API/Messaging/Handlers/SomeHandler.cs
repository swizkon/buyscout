using System.Globalization;
using System.Threading.Tasks;
using BuyScout.Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace BuyScout.API.Messaging.Handlers
{
    public class SomeHandler :
        IConsumer<AddItemToListCommand>,
        IConsumer<SystemTickEvent>,
        IConsumer<SystemHeartbeatEvent>
    {
        private readonly IHubContext<TestHub> _hubContext;
        private readonly ILogger<SomeHandler> _logger;

        public SomeHandler(
            IHubContext<TestHub> hubContext,
            ILogger<SomeHandler> logger)
        {
            _logger = logger;
            _hubContext = hubContext;
        }

        public Task Consume(ConsumeContext<AddItemToListCommand> context)
        {
            var message = context.Message;
            _logger.LogInformation("{MessageType} Title {Title}", message.GetType().Name, message.Title);
            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<SystemTickEvent> context)
        {
            var message = context.Message;
            // throw new System.NotImplementedException();

            _logger.LogInformation("{MessageType}:{CorrelationId} at {Timestamp}", message.GetType().Name, context.CorrelationId, message.UtcTimestamp);
            return Task.CompletedTask;
        }

        public async Task Consume(ConsumeContext<SystemHeartbeatEvent> context)
        {
            await _hubContext.Clients.All.SendCoreAsync("SystemHeartbeatEvent", new[]
            {
                context.Message.Name,
                context.Message.UtcTimestamp.ToString(CultureInfo.InvariantCulture)
            });
            //await _hubContext.Clients.All.SendCoreAsync("")
            // throw new System.NotImplementedException();
        }
    }
}
