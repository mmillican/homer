using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Homer.Shared.Entities.Shopping
{
    public class ShoppingItem
    {
        public int Id { get; set; }

        public int ListId { get; set; }
        [ForeignKey(nameof(ListId))]
        public virtual ShoppingList List { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? PurchasedOn { get; set; }
    }
}
