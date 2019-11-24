using AutoMapper;
using AutoMapper.QueryableExtensions;
using Homer.Api.Models.Shopping;
using Homer.Shared.Data;
using Homer.Shared.Entities.Shopping;
using Homer.Shared.Entities.TaskLists;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Homer.Api.Controllers
{
    [Route("/shopping-lists/{listId}/items")]
    [ApiController]
    public class ShoppingItemsController : ControllerBase
    {
        private readonly IRepository<ShoppingList> _shoppingListRepository;
        private readonly IRepository<ShoppingItem> _shoppingItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ShoppingItemsController> _logger;

        public ShoppingItemsController(IRepository<ShoppingList> shoppingListRepository,
            IRepository<ShoppingItem> shoppingItemRepository,
            IMapper mapper,
            ILogger<ShoppingItemsController> logger)
        {
            _shoppingListRepository = shoppingListRepository;
            _shoppingItemRepository = shoppingItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingItemModel>>> GetItems([FromRoute] int listId, [FromQuery] bool? purchased = false)
        {
            var items = await _shoppingItemRepository.Table
                .Where(x => x.ListId == listId
                    && (!purchased.HasValue || x.PurchasedOn.HasValue == purchased.Value))
                .ProjectTo<ShoppingItemModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingItemModel>> GetItem(int listId, int id)
        {
            var item = await _shoppingItemRepository.GetByIdAsync(id);
            if (item == null || item.ListId != listId)
            {
                return NotFound();
            }

            var model = _mapper.Map<ShoppingItemModel>(item);
            return model;
        }

        [HttpPost]
        public async Task<ActionResult<TodoList>> Create(int listId, [Bind("Name", "Description")] ShoppingItemModel model)
        {
            var listExists = await _shoppingListRepository.Table.AnyAsync(x => x.Id == listId);
            if (!listExists)
            {
                _logger.LogInformation("List {id} does not exist", listId);
                return NotFound();
            }

            try
            {
                var item = new ShoppingItem
                {
                    ListId = listId,
                    Name = model.Name,
                    Description = model.Description,
                    CreatedOn = DateTime.UtcNow,
                };

                await _shoppingItemRepository.CreateAsync(item);

                _logger.LogInformation("Created shopping item");

                model = _mapper.Map<ShoppingItemModel>(item);
                return CreatedAtAction(nameof(GetItem), new { listId, id = model.Id }, model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating shopping item");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int listId, int id, ShoppingItemModel model)
        {
            try
            {
                var item = await _shoppingItemRepository.GetByIdAsync(id);
                if (item == null || item.ListId != listId)
                {
                    return NotFound();
                }

                item.Name = model.Name;
                item.Description = model.Description;
                item.PurchasedOn = model.PurchasedOn;

                await _shoppingItemRepository.UpdateAsync(item);

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
        public async Task<IActionResult> Delete(int listId, int id)
        {
            try
            {
                var item = await _shoppingItemRepository.GetByIdAsync(id);
                if (item == null || item.ListId != listId)
                {
                    return NotFound();
                }

                await _shoppingItemRepository.DeleteAsync(item);

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
