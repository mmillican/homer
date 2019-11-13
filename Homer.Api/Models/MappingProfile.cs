using AutoMapper;
using Homer.Api.Models.Meals;
using Homer.Api.Models.Shopping;
using Homer.Api.Models.Tasks;
using Homer.Shared.Entities.Meals;
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

            CreateMap<Meal, MealModel>()
                .ForMember(x => x.PrepEffort, map => map.MapFrom(x => (int)x.PrepEffort))
                .ForMember(x => x.PrepEffortName, map => map.MapFrom(x => x.PrepEffort.ToString("G")));
        }
    }
}
