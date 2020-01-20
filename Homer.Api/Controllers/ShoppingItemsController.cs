using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Homer.Shared.Entities.Shopping;
using Homer.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homer.Api.Controllers
{
    [Route("/shopping-lists/{listId}/items")]
    [ApiController]
    public class ShoppingItemsController : ControllerBase
    {
        private readonly IDataContext _dataContext;
        private readonly ILogger<ShoppingItemsController> _logger;

        public ShoppingItemsController(IDataContext dataContext,
            ILogger<ShoppingItemsController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingItem>>> GetItems([FromRoute] Guid listId, [FromQuery] bool? purchased = false)
        {
            var scanConditions = new List<ScanCondition>
            {
                new ScanCondition(nameof(ShoppingItem.ListId), ScanOperator.Equal, listId.ToString())
            };

            if (purchased.HasValue && purchased.Value)
            {
                scanConditions.Add(new ScanCondition(nameof(ShoppingItem.PurchasedOn), ScanOperator.IsNotNull));
            }
            else if (purchased.HasValue && !purchased.Value)
            {
                scanConditions.Add(new ScanCondition(nameof(ShoppingItem.PurchasedOn), ScanOperator.IsNull));
            }

            var items = await _dataContext.GetAsync<ShoppingItem>(scanConditions);

            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingItem>> GetItem(Guid listId, Guid id)
        {
            var item = await _dataContext.GetByIdAsync<ShoppingItem>(id.ToString());
            if (item == null || item.ListId != listId.ToString())
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<ShoppingItem>> Create(Guid listId, [Bind("Name", "Description")] ShoppingItem model)
        {
            var shoppingList = await _dataContext.GetByIdAsync<ShoppingList>(listId.ToString());
            if (shoppingList == null) 
            {
                return NotFound("List does not exist");
            }

            try
            {
                var item = new ShoppingItem
                {
                    ListId = listId.ToString(),
                    Name = model.Name,
                    Description = model.Description,
                    CreatedOn = DateTime.UtcNow,
                };

                await _dataContext.SaveAsync(item);
                model.Id = item.Id;

                _logger.LogInformation("Created shopping item");

                return CreatedAtAction(nameof(GetItem), new { listId, id = model.Id }, model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating shopping item");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid listId, Guid id, ShoppingItem model)
        {
            try
            {
                var item = await _dataContext.GetByIdAsync<ShoppingItem>(id.ToString());
                if (item == null || item.ListId != listId.ToString())
                {
                    return NotFound();
                }

                item.Name = model.Name;
                item.Description = model.Description;
                item.PurchasedOn = model.PurchasedOn;

                await _dataContext.SaveAsync(item);

                _logger.LogInformation("Updated shopping item {id}", id);

                return Ok(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating shopping item {id}", id);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid listId, Guid id)
        {
            try
            {
                var item = await _dataContext.GetByIdAsync<ShoppingItem>(id.ToString());
                if (item == null || item.ListId != listId.ToString())
                {
                    return NotFound();
                }

                await _dataContext.DeleteAsync(item);

                _logger.LogInformation("Deleted shopping item {id}", id);

                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting shopping item {id}", id);
                return StatusCode(500);
            }
        }
    }
}
