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
using System.Linq;
using System.Threading.Tasks;

namespace Homer.Api.Controllers
{
    [Route("/todo-lists/{listId}/items")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly IRepository<TodoList> _todoListRepository;
        private readonly IRepository<TodoItem> _todoItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TodosController> _logger;

        public TodosController(IRepository<TodoList> todoListRepository,
            IRepository<TodoItem> todoItemRepository,
            IMapper mapper,
            ILogger<TodosController> logger)
        {
            _todoListRepository = todoListRepository;
            _todoItemRepository = todoItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemModel>>> GetTodoItems(int listId)
        {
            var todos = await _todoItemRepository.Table
                .Where(x => x.ListId == listId)
                .ProjectTo<TodoItemModel>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return todos;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemModel>> GetTodo(int listId, int id)
        {
            var todo = await _todoItemRepository.GetByIdAsync(id);
            if (todo == null || todo.ListId != listId)
            {
                return NotFound();
            }

            var model = _mapper.Map<TodoItemModel>(todo);
            return model;
        }

        [HttpPost("")]
        public async Task<ActionResult<TodoItemModel>> Create(int listId, TodoItemModel model)
        {
            var listExists = await _todoListRepository.Table.AnyAsync(x => x.Id == listId);
            if (!listExists)
            {
                _logger.LogInformation("List {id} does not exist", listId);
                return NotFound();
            }

            try
            {
                var todo = new TodoItem
                {
                    ListId = listId,
                    Title = model.Title,
                    Description = model.Description,
                    CreatedOn = model.CreatedOn,
                    DueOn = model.DueOn
                };

                await _todoItemRepository.CreateAsync(todo);

                _logger.LogInformation("Created todo");

                model = _mapper.Map<TodoItemModel>(todo);
                return CreatedAtAction(nameof(GetTodo), new { listId, id = model.Id }, model);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error creating todo");
                return StatusCode(500);                
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int listId, int id, TodoItemModel model)
        {
            try
            {
                var todo = await _todoItemRepository.GetByIdAsync(id);
                if (todo == null || todo.ListId != listId)
                {
                    return NotFound();
                }

                todo.Title = model.Title;
                todo.Description = model.Description;
                todo.CreatedOn = DateTime.UtcNow;
                todo.DueOn = model.DueOn;

                await _todoItemRepository.UpdateAsync(todo);

                _logger.LogInformation("Updated todo {id}", id);

                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating todo {id}", id);
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> Delete(int listId, int id)
        {
            try
            {
                var todo = await _todoItemRepository.GetByIdAsync(id);
                if (todo == null || todo.ListId != listId)
                {
                    return NotFound();
                }

                await _todoItemRepository.DeleteAsync(todo);

                _logger.LogInformation("Deleted todo {id}", id);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting todo {id}", id);
                return StatusCode(500);
            }
        }
    }
}
