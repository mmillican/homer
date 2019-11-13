using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homer.Api.Models.Meals
{
    public class MealModel
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int PrepEffort { get; set; }
        public string PrepEffortName { get; internal set; }

        public bool IsFavorite { get; set; }
        public bool IsKidFriendly { get; set; }
    }
}
