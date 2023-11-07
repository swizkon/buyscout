using System;
using System.Threading.Tasks;
using BuyScout.Contracts;
using MassTransit;
using MassTransit.Riders;
using MassTransit.Util;
using Microsoft.Extensions.Logging;

namespace BuyScout.Processors
{
    public class BlaBlaSomeHandler :
        IConsumer<AddItemToListCommand>,
        IConsumer<SystemTickEvent>,
        IConsumer<CheckOrderStatus>
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

        public async Task Consume(ConsumeContext<CheckOrderStatus> context)
        {
            var timestamp = DateTime.UtcNow;

            if (timestamp.Second % 2 == 0)
            {
                throw new Exception("Cant use even seconds");
            }

            await context.RespondAsync<OrderStatusResult>(new
            {
                context.Message.OrderId,
                Timestamp = timestamp,
                Status = "Alles gut"
            });
        }
    }
}