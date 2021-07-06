using System.Threading.Tasks;
using BuyScout.Contracts;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace BuyScout.Processors
{
    public class BlaBlaSomeHandler :
        IConsumer<AddItemToListCommand>,
        IConsumer<SystemTickEvent>
    {
        private readonly ILogger<BlaBlaSomeHandler> _logger;

        public BlaBlaSomeHandler(ILogger<BlaBlaSomeHandler> logger)
        {
            _logger = logger;
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

            _logger.LogInformation("{MessageType}:{CorrelationId} at {Timestamp}", message.GetType().Name,
                context.CorrelationId, message.UtcTimestamp);
            return Task.CompletedTask;
        }
    }
}