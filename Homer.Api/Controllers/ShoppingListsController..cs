using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Homer.Shared.Entities.Shopping;
using Homer.Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homer.Api.Controllers
{
    [Route("/shopping-lists")]
    [ApiController]
    public class ShoppingListsController : ControllerBase
    {
        private readonly IDataContext _dataContext;
        private readonly ILogger<ShoppingListsController> _logger;

        public ShoppingListsController(IDataContext dataContext,
            ILogger<ShoppingListsController> logger)
        {
            _dataContext = dataContext;
            _logger = logger;
        }

        // GET: /TaskLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingList>>> GetLists()
        {
            var lists = await _dataContext.GetAsync<ShoppingList>();

            return Ok(lists);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingList>> GetList(Guid id)
        {
            var shoppingList = await _dataContext.GetByIdAsync<ShoppingList>(id.ToString());
            if (shoppingList == null)
            {
                return NotFound();
            }

            return Ok(shoppingList);
        }

        // POST: /TaskLists
        [HttpPost]
        public async Task<ActionResult<ShoppingList>> Create(ShoppingList model)
        {
            try
            {
                var shoppingList = new ShoppingList
                {
                    Name = model.Name,
                    OwnerId = 1 // TODO: Set from current user
                };
                
                await _dataContext.SaveAsync(shoppingList);

                _logger.LogInformation("Created shopping list");

                model.Id = shoppingList.Id;

                return CreatedAtAction(nameof(GetList), new { id = model.Id }, model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating shopping list");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, ShoppingList model)
        {
            try
            {
                var shoppingList = await _dataContext.GetByIdAsync<ShoppingList>(id.ToString());
                if (shoppingList == null)
                {
                    return NotFound();
                }

                shoppingList.Name = model.Name;

                await _dataContext.SaveAsync(shoppingList);

                _logger.LogInformation("Updated shopping list {id}", id);

                return Ok(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating shopping list {id}", id);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (await DoesListHaveItems(id))
            {
                _logger.LogInformation("Could not delete shopping list {id} because it has items associated to it.", id);

                ModelState.AddModelError(nameof(id), "Cannot delete list with items associated to it");
                return BadRequest(ModelState);
            }

            try
            {
                var shoppingList = await _dataContext.GetByIdAsync<ShoppingList>(id.ToString());
                if (shoppingList == null)
                {
                    return NotFound();
                }

                await _dataContext.DeleteAsync(shoppingList);

                _logger.LogInformation("Deleted shopping list {id}", id);

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting shopping list {id}", id);
                return StatusCode(500);
            }
        }

        private async Task<bool> DoesListHaveItems(Guid listId)
        {
            var conditions = new List<ScanCondition>
            {
                new ScanCondition(nameof(ShoppingItem.ListId), ScanOperator.Equal, listId.ToString())
            };

            var items = await _dataContext.GetAsync<ShoppingItem>(conditions);
            return items.Any();
        }
    }
}
