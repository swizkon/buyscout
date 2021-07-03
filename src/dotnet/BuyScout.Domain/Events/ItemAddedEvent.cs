using System;

namespace BuyScout.Domain.Events
{
    public class ItemAddedEvent
    {
        public Guid ListId { get; set; }
        public string Title { get; set; }
    }
}