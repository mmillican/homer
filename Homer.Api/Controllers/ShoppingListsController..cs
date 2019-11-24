using AutoMapper;
using AutoMapper.QueryableExtensions;
using Homer.Api.Models.Shopping;
using Homer.Shared.Data;
using Homer.Shared.Entities.Shopping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homer.Api.Controllers
{
    [Route("/shopping-lists")]
    [ApiController]
    public class ShoppingListsController : ControllerBase
    {
        private readonly IRepository<ShoppingList> _shoppingListRepository;
        private readonly IRepository<ShoppingItem> _shoppingItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ShoppingListsController> _logger;

        public ShoppingListsController(IRepository<ShoppingList> shoppingListRepository,
            IRepository<ShoppingItem> shoppingItemRepository,
            IMapper mapper,
            ILogger<ShoppingListsController> logger)
        {
            _shoppingListRepository = shoppingListRepository;
            _shoppingItemRepository = shoppingItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: /TaskLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingListModel>>> GetLists()
        {
            var lists = await _shoppingListRepository.Table
                .ProjectTo<ShoppingListModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return lists;
        }

        // GET: /TaskLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShoppingListModel>> GetList(int id)
        {
            var shoppingList = await _shoppingListRepository.GetByIdAsync(id);
            if (shoppingList == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<ShoppingListModel>(shoppingList);
            return model;
        }

        // POST: /TaskLists
        [HttpPost]
        public async Task<ActionResult<ShoppingListModel>> Create(ShoppingListModel model)
        {
            try
            {
                var shoppingList = new ShoppingList
                {
                    Name = model.Name,
                    OwnerId = 1 // TODO: Set from current user
                };

                await _shoppingListRepository.CreateAsync(shoppingList);

                _logger.LogInformation("Created shopping list");

                model = _mapper.Map<ShoppingListModel>(shoppingList);
                return CreatedAtAction(nameof(GetList), new { id = model.Id }, model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating shopping list");
                return StatusCode(500);
            }
        }

        // PUT: /TaskLists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ShoppingListModel model)
        {
            try
            {
                var shoppingList = await _shoppingListRepository.GetByIdAsync(id);
                if (shoppingList == null)
                {
                    return NotFound();
                }

                shoppingList.Name = model.Name;

                await _shoppingListRepository.UpdateAsync(shoppingList);

                _logger.LogInformation("Updated shopping list {id}", id);

                return Ok(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating shopping list {id}", id);
                return StatusCode(500);
            }
        }

        // DELETE: /TaskLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var hasItems = await _shoppingItemRepository.Table.AnyAsync(x => x.ListId == id);
            if (hasItems)
            {
                _logger.LogInformation("Could not delete shopping list {id} because it has items associated to it.", id);

                ModelState.AddModelError(nameof(id), "Cannot delete list with items associated to it");
                return BadRequest(ModelState);
            }

            try
            {
                var shoppingList = await _shoppingListRepository.GetByIdAsync(id);
                if (shoppingList == null)
                {
                    return NotFound();
                }

                await _shoppingListRepository.DeleteAsync(shoppingList);

                _logger.LogInformation("Deleted shopping list {id}", id);

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting shopping list {id}", id);
                return StatusCode(500);
            }
        }
    }
}
