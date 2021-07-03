using System;

namespace BuyScout.Domain.Events
{
    public class ItemRemovedEvent
    {
        public Guid ItemId { get; set; }
    }
}