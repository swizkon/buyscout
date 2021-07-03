using System;

namespace BuyScout.Domain.Events
{
    public class ItemCompletedEvent
    {
        public Guid ItemId { get; set; }
    }
}