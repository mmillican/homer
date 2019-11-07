using AutoMapper;
using Homer.Api.Models.Shopping;
using Homer.Api.Models.Tasks;
using Homer.Shared.Entities.Shopping;
using Homer.Shared.Entities.TaskLists;

namespace Homer.Api.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoList, TodoListModel>();
            CreateMap<TodoItem, TodoItemModel>();

            CreateMap<ShoppingList, ShoppingListModel>();
            CreateMap<ShoppingItem, ShoppingItemModel>();
        }
    }
}
