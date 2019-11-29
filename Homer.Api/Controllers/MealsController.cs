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
    [Route("/meals")]
    [ApiController]
    public class MealsController : ControllerBase
    {
        private readonly IRepository<Meal> _mealsRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MealsController> _logger;

        public MealsController(IRepository<Meal> mealsRepository,
            IMapper mapper,
            ILogger<MealsController> logger)
        {
            _mealsRepository = mealsRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MealModel>>> GetMeals()
        {
            var meals = await _mealsRepository.Table
                .OrderBy(x => x.Name)
                .ProjectTo<MealModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return meals;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MealModel>> GetById(int id)
        {
            var meal = await _mealsRepository.GetByIdAsync(id);
            if (meal == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<MealModel>(meal);
            return model;
        }

        [HttpPost]
        public async Task<ActionResult<MealModel>> Create(MealModel model)
        {
            try
            {
                var meal = new Meal
                {
                    Name = model.Name,
                    Description = model.Description,
                    PrepEffort = (MealPrepEffort)model.PrepEffort,
                    IsFavorite = model.IsFavorite,
                    IsKidFriendly = model.IsKidFriendly
                };

                await _mealsRepository.CreateAsync(meal);

                _logger.LogInformation("Created meal");

                model = _mapper.Map<MealModel>(meal);
                return CreatedAtAction("GetById", new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating meal");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, MealModel model)
        {
            try
            {
                var meal = await _mealsRepository.GetByIdAsync(id);
                if (meal == null)
                {
                    return NotFound();
                }

                meal.Name = model.Name;
                meal.Description = model.Description;
                meal.PrepEffort = (MealPrepEffort)model.PrepEffort;
                meal.IsFavorite = model.IsFavorite;
                meal.IsKidFriendly = model.IsKidFriendly;

                await _mealsRepository.UpdateAsync(meal);

                _logger.LogInformation("Updated meal {id}", id);

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating meal {id}", id);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var meal = await _mealsRepository.GetByIdAsync(id);
                if (meal == null)
                {
                    return NotFound();
                }

                await _mealsRepository.DeleteAsync(meal);

                _logger.LogInformation("Deleted meal {id}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting meal {id}", id);
                return StatusCode(500);
            }
        }
    }
}
