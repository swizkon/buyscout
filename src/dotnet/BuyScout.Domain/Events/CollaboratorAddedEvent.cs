using System;

namespace BuyScout.Domain.Events
{
    public class CollaboratorAddedEvent
    {
        public Guid ListId { get; set; }
        public string Email { get; set; }
    }
}