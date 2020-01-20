using System;
using System.ComponentModel.DataAnnotations;

namespace Homer.Shared.Entities.Shopping
{
    public class ShoppingItem
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ListId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? PurchasedOn { get; set; }
    }
}
