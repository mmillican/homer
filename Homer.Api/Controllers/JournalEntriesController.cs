using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using AutoMapper;
using Homer.Shared.Entities.Journal;
using Homer.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var userId = User.FindFirstValue(ClaimTypes.Email);

            var conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition(nameof(JournalEntry.UserId), ScanOperator.Equal, userId));

            var entries = await _dataContext.GetAsync<JournalEntry>(conditions);
            entries = entries.OrderByDescending(x => x.Date);

            return Ok(entries);
        }

        [HttpGet("{date}")]
        public async Task<ActionResult<IEnumerable<JournalEntry>>> GetJournalEntriesForDate(DateTime date)
        {
            var userId = User.FindFirstValue(ClaimTypes.Email);

            var conditions = new List<ScanCondition>();
            conditions.Add(new ScanCondition(nameof(JournalEntry.UserId), ScanOperator.Equal, userId));
            conditions.Add(new ScanCondition(nameof(JournalEntry.Date), ScanOperator.Equal, date));

            var entries = await _dataContext.GetAsync<JournalEntry>(conditions);
            return Ok(entries);
        }

        [HttpPost("")]
        public async Task<ActionResult<JournalEntry>> Create(JournalEntry model)
        {
            try
            {
                var entry = new JournalEntry();
                entry.UserId = User.FindFirstValue(ClaimTypes.Email);
                entry.Date = model.Date;
                entry.Mood = model.Mood;
                entry.Personal = model.Personal;
                entry.Work = model.Work;

                await _dataContext.SaveAsync(entry);

                model.Id = entry.Id;
                model.UserId = entry.UserId;
                model.Date = entry.Date;

                _logger.LogInformation("Created journal entry");

                return CreatedAtAction("GetJournalEntriesForDate", new { date = model.Date }, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating journal entry");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JournalEntry>> Update(Guid id, JournalEntry model)
        {
            try
            {
                var entry = await _dataContext.GetByIdAsync<JournalEntry>(id.ToString());
                if (entry == null)
                {
                    return NotFound();
                }
                
                entry.Mood = model.Mood;
                entry.Personal = model.Personal;
                entry.Work = model.Work;

                await _dataContext.SaveAsync(entry);

                model.Id = entry.Id;
                model.UserId = entry.UserId;
                model.Date = entry.Date;

                _logger.LogInformation("Updated journal entry");

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating journal entry");
                return StatusCode(500);
            }
        }
        
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
