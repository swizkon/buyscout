using System;

namespace BuyScout.Domain.Events
{
    public class ListCreatedEvent
    {
        public Guid ListId { get; set; }
        public string Title { get; set; }
    }
}
