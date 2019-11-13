using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homer.Shared.Entities.Meals
{
    public class Meal
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public MealPrepEffort PrepEffort { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsKidFriendly { get; set; }
    }

    public class ScheduledMeal
    {
        public int Id { get; set; }

        public DateTime MealDate { get; set; }
        public MealTime MealTime { get; set; }

        public int MealId { get; set; }
        [ForeignKey(nameof(MealId))]
        public virtual Meal Meal { get; set; }
    }

    public enum MealTime
    {
        Breakfast = 1,
        Lunch = 2,
        Dinner = 3
    }


    public enum MealPrepEffort
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
}
