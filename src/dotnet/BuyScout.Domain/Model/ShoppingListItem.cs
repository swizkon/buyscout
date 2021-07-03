using System;

namespace BuyScout.Domain.Model
{
    public class ShoppingListItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Title { get; set; }

    }
}
