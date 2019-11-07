using Homer.Shared.Entities.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homer.Shared.Entities.TaskLists
{
    public class TodoList
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public virtual User Owner { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
