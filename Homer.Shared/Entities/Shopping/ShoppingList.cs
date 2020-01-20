using System;
using System.ComponentModel.DataAnnotations;

namespace Homer.Shared.Entities.Shopping
{
    public class ShoppingList
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public int OwnerId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
