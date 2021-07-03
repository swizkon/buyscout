using System;

namespace BuyScout.Domain.Model
{
    public class Collaborator
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public string Status { get; set; }

    }
}