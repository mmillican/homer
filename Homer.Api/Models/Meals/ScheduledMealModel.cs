using Homer.Shared.Entities.Meals;
using System;

namespace Homer.Api.Models.Meals
{
    public class ScheduledMealModel
    {
        public int Id { get; set; }
        
        // Treating as a string here because we don't care about times
        public string MealDate { get; set; }

        public int MealTimeId { get; set; }
        public string MealTime { get; internal set; }

        public int MealId { get; set; }
        public MealModel Meal { get; internal set; }
    }
}
