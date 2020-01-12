using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using Homer.Shared.Entities.Contacts;
using Homer.Shared.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homer.Api.Controllers
{
    [Route("/addresses")]
    [ApiController]
    // [AllowAnonymous]
    public class AddressesController : ControllerBase
    {
        private readonly IDataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogger<AddressesController> _logger;

        public AddressesController(IDataContext dataContext,
            IMapper mapper,
            ILogger<AddressesController> logger)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Address>>> GetAddresses()
        {
            var conditions = new List<ScanCondition>();

            var entries = await _dataContext.GetAsync<Address>(conditions);
            entries = entries.OrderBy(x => x.LastName).ThenBy(x => x.FirstName);

            return Ok(entries);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Address>> GetAddress(Guid id)
        {
            var address = await _dataContext.GetByIdAsync<Address>(id.ToString());
            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpPost("")]
        public async Task<ActionResult<Address>> Create(Address model)
        {
            try
            {
                var address = new Address();
                address.LastName = model.LastName;
                address.FirstName = model.FirstName;
                address.FormalNames = model.FormalNames;
                address.AddressLine1 = model.AddressLine1;
                address.AddressLine2 = model.AddressLine2;
                address.City = model.City;
                address.State = model.State;
                address.ZipCode = model.ZipCode;
                address.LastUpdated = model.LastUpdated;
                address.NeedsUpdate = model.NeedsUpdate;

                await _dataContext.SaveAsync(address);

                model.Id = address.Id;

                _logger.LogInformation("Created address");

                return CreatedAtAction("GetAddress", new { id = model.Id }, model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating address");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Address>> Update(Guid id, Address model)
        {
            try
            {
                var address = await _dataContext.GetByIdAsync<Address>(id.ToString());
                if (address == null)
                {
                    return NotFound();
                }
                
                address.LastName = model.LastName;
                address.FirstName = model.FirstName;
                address.FormalNames = model.FormalNames;
                address.AddressLine1 = model.AddressLine1;
                address.AddressLine2 = model.AddressLine2;
                address.City = model.City;
                address.State = model.State;
                address.ZipCode = model.ZipCode;
                address.LastUpdated = model.LastUpdated;
                address.NeedsUpdate = model.NeedsUpdate;

                await _dataContext.SaveAsync(address);

                _logger.LogInformation("Updated address");

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating address");
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
