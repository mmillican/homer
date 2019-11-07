using System.ComponentModel.DataAnnotations;

namespace Homer.Api.Models.Shopping
{
    public class ShoppingListModel
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
