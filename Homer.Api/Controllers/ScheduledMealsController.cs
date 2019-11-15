using AutoMapper;
using AutoMapper.QueryableExtensions;
using Homer.Api.Models.Meals;
using Homer.Shared.Data;
using Homer.Shared.Entities.Meals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homer.Api.Controllers
{
    [Route("api/meals/scheduled")]
    [ApiController]
    public class ScheduledMealsController : ControllerBase
    {
        private readonly IRepository<ScheduledMeal> _scheduledMealRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ScheduledMealsController> _logger;

        public ScheduledMealsController(IRepository<ScheduledMeal> mealsRepository,
            IMapper mapper,
            ILogger<ScheduledMealsController> logger)
        {
            _scheduledMealRepository = mealsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduledMealModel>>> GetMeals(DateTime? startDate = null, DateTime? endDate = null)
        {
            var meals = await _scheduledMealRepository.Table
                .Include(x => x.Meal)
                .Where(x => (!startDate.HasValue || x.MealDate >= startDate.Value)
                    && (!endDate.HasValue || x.MealDate <= endDate.Value))
                .ProjectTo<ScheduledMealModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return meals;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ScheduledMealModel>> GetById(int id)
        {
            var meal = await _scheduledMealRepository.GetByIdAsync(id);
            if (meal == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ScheduledMealModel>(meal);
            return model;
        }

        [HttpPost]
        public async Task<ActionResult<MealModel>> Create(ScheduledMealModel model)
        {
            try
            {
                // TODO: Validate meal exists
                var scheduledMeal = new ScheduledMeal
                {
                    MealDate = DateTime.Parse(model.MealDate),
                    MealTime = (MealTime)model.MealTimeId,
                    MealId = model.MealId
                };

                await _scheduledMealRepository.CreateAsync(scheduledMeal);
                
                _logger.LogInformation("Created scheduled meal");

                // YUCK! - Get the record back from the DB so it's populated with the nav prop
                var newScheduledMeal = await _scheduledMealRepository.Table.Include(x => x.Meal).SingleOrDefaultAsync(x => x.Id == scheduledMeal.Id);

                model = _mapper.Map<ScheduledMealModel>(newScheduledMeal);
                return CreatedAtAction("GetById", new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating scheduled meal");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ScheduledMealModel model)
        {
            try
            {
                var scheduledMeal = await _scheduledMealRepository.GetByIdAsync(id);
                if (scheduledMeal == null)
                {
                    return NotFound();
                }

                // TODO: Validate meal exists
                scheduledMeal.MealDate = DateTime.Parse(model.MealDate);
                scheduledMeal.MealTime = (MealTime)model.MealTimeId;
                scheduledMeal.MealId = model.MealId;

                await _scheduledMealRepository.UpdateAsync(scheduledMeal);

                _logger.LogInformation("Updated scheduled meal {id}", id);

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating scheduled meal {id}", id);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var meal = await _scheduledMealRepository.GetByIdAsync(id);
                if (meal == null)
                {
                    return NotFound();
                }

                await _scheduledMealRepository.DeleteAsync(meal);

                _logger.LogInformation("Deleted scheduled meal {id}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting scheduled meal {id}", id);
                return StatusCode(500);
            }
        }
    }
}
