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
}
