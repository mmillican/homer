using System;
using System.ComponentModel.DataAnnotations;

namespace Homer.Api.Models.Tasks
{
    public class TodoItemModel
    {
        public int Id { get; set; }

        public int ListId { get; set; }

        [Required, MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? DueOn { get; set; }
    }
}
