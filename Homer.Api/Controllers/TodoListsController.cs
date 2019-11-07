using AutoMapper;
using AutoMapper.QueryableExtensions;
using Homer.Api.Models.Tasks;
using Homer.Shared.Data;
using Homer.Shared.Entities.TaskLists;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Homer.Api.Controllers
{
    [Route("api/todo-lists")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly IRepository<TodoList> _todoListRepository;
        private readonly IRepository<TodoItem> _todoItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TodoListsController> _logger;

        public TodoListsController(IRepository<TodoList> todoListRepository,
            IRepository<TodoItem> todoItemRepository,
            IMapper mapper,
            ILogger<TodoListsController> logger)
        {
            _todoListRepository = todoListRepository;
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListModel>>> GetLists()
        {
            var lists = await _todoListRepository.Table
                .ProjectTo<TodoListModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return lists;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListModel>> GetList(int id)
        {
            var todoList = await _todoListRepository.GetByIdAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<TodoListModel>(todoList);
            return model;
        }

        [HttpPost]
        public async Task<ActionResult<TodoListModel>> Create(TodoListModel model)
        {
            try
            {
                var todoList = new TodoList
                {
                    Name = model.Name,
                    OwnerId = 1 // TODO: Set from current user
                };

                await _todoListRepository.CreateAsync(todoList);

                _logger.LogInformation("Created todo list");

                model = _mapper.Map<TodoListModel>(todoList);
                return CreatedAtAction("GetList", new { id = model.Id }, model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating todo list");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TodoListModel model)
        {
            try
            {
                var todoList = await _todoListRepository.GetByIdAsync(id);
                if (todoList == null)
                {
                    return NotFound();
                }

                todoList.Name = model.Name;

                await _todoListRepository.UpdateAsync(todoList);

                _logger.LogInformation("Updated todo list {id}", id);

                return Ok(model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating todo list {id}", id);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTaskList(int id)
        {
            var hasTodos = await _todoItemRepository.Table.AnyAsync(x => x.ListId == id);
            if (hasTodos)
            {
                _logger.LogInformation("Could not delete todo list {id} because it has items associated to it.", id);

                ModelState.AddModelError(nameof(id), "Cannot delete list with items associated to it");
                return BadRequest(ModelState);
            }

            try
            {
                var todoList = await _todoListRepository.GetByIdAsync(id);
                if (todoList == null)
                {
                    return NotFound();
                }

                await _todoListRepository.DeleteAsync(todoList);

                _logger.LogInformation("Deleted todo list {id}", id);

                return NoContent();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error deleting todo list {id}", id);
                return StatusCode(500);
            }
        }
    }
}
