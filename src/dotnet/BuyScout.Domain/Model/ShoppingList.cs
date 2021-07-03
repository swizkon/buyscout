using System;
using System.Collections.Generic;

namespace BuyScout.Domain.Model
{
    public class ShoppingList
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public ICollection<Collaborator> Collaborators { get; set; }
    }
}