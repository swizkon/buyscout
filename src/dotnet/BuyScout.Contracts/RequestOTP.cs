using System;

namespace BuyScout.Contracts
{
    public class AddItemToListCommand
    {
        public string ListId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class ItemAcceptedEvent
    {
    }


    public class SystemTickEvent
    {
        public DateTime UtcTimestamp { get; set; }
    }


    public class SystemHeartbeatEvent
    {
        public DateTime UtcTimestamp { get; set; }
        public string Name { get; set; }
    }

    public record CheckOrderStatus
    {
        public string OrderId { get; init; }
    }

    public record OrderStatusResult
    {
        public string OrderId { get; init; }
        public DateTime Timestamp { get; init; }
        public string Status { get; init; }
    }
}
