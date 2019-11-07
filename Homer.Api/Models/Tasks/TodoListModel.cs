using System.ComponentModel.DataAnnotations;

namespace Homer.Api.Models.Tasks
{
    public class TodoListModel
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
    }
}
