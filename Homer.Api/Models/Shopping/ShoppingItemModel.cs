using System;
using System.ComponentModel.DataAnnotations;

namespace Homer.Api.Models.Shopping
{
    public class ShoppingItemModel
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? PurchasedOn { get; set; }
    }
}
