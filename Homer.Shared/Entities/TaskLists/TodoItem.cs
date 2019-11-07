using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homer.Shared.Entities.TaskLists
{
    public class TodoItem
    {
        public int Id { get; set; }

        public int ListId { get; set; }
        [ForeignKey(nameof(ListId))]
        public virtual TodoList List { get; set; }

        [Required, MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? DueOn { get; set; }
    }
}
