using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using Homer.Shared.Entities.Journal;
using Homer.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Homer.Api.Controllers
{
    [Route("/journal-entries")]
    [ApiController]
    public class JournalEntriesController : ControllerBase
    {
        private readonly IDataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogger<JournalEntriesController> _logger;

        public JournalEntriesController(IDataContext dataContext,
            IMapper mapper,
            ILogger<JournalEntriesController> logger)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<JournalEntry>>> GetJournalEntries()
        {
            var conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition(nameof(JournalEntry.UserId), ScanOperator.Equal, User.Identity.Name));

            var entries = await _dataContext.GetAsync<JournalEntry>(conditions);
            return Ok(entries);
        }

        [HttpGet("date/{date}")]
        public async Task<ActionResult<IEnumerable<JournalEntry>>> GetJournalEntriesForDate(DateTime date)
        {
            var conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition(nameof(JournalEntry.UserId), ScanOperator.Equal, User.FindFirstValue(ClaimTypes.NameIdentifier)));
            conditions.Add(new ScanCondition(nameof(JournalEntry.Date), ScanOperator.Equal, date));

            var entries = await _dataContext.GetAsync<JournalEntry>(conditions);
            return Ok(entries);
        }

        [HttpPost]
        public async Task<ActionResult<JournalEntry>> Create(JournalEntry model)
        {
            try
            {
                model.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                model.Date = model.Date.Date;

                await _dataContext.SaveAsync(model);

                _logger.LogInformation("Created journal entry");

                return CreatedAtAction("GetJournalEntriesForDate", new { date = model.Date }, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating journal entry");
                return StatusCode(500);
            }
        }

        // [HttpPut("{id}")]
        // public async Task<IActionResult> Update(int id, MealModel model)
        // {
        //     try
        //     {
        //         var meal = await _mealsRepository.GetByIdAsync(id);
        //         if (meal == null)
        //         {
        //             return NotFound();
        //         }

        //         meal.Name = model.Name;
        //         meal.Description = model.Description;
        //         meal.PrepEffort = (MealPrepEffort)model.PrepEffort;
        //         meal.IsFavorite = model.IsFavorite;
        //         meal.IsKidFriendly = model.IsKidFriendly;

        //         await _mealsRepository.UpdateAsync(meal);

        //         _logger.LogInformation("Updated meal {id}", id);

        //         return Ok(model);
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error updating meal {id}", id);
        //         return StatusCode(500);
        //     }
        // }

        // [HttpDelete("{id}")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     try
        //     {
        //         var meal = await _mealsRepository.GetByIdAsync(id);
        //         if (meal == null)
        //         {
        //             return NotFound();
        //         }

        //         await _mealsRepository.DeleteAsync(meal);

        //         _logger.LogInformation("Deleted meal {id}", id);

        //         return NoContent();
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error deleting meal {id}", id);
        //         return StatusCode(500);
        //     }
        // }
    }
}
