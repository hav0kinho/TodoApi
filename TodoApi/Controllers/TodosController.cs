using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;
using TodoApi.Filters.ActionFilters;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TodosController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetTodos()
        {
            return Ok(_db.Todos.ToList());
        }

        [HttpGet("{id}")]
        [TypeFilter(typeof(Todo_ValidateTodoIdFilterAttribute))]
        public IActionResult GetTodoById(int id)
        {
            return Ok(HttpContext.Items["todo"]);
        }

        [HttpPost]
        [TypeFilter(typeof(Todo_ValidateCreateTodoFilterAttribute))]
        public IActionResult CreateTodo(Todo todo)
        {
            _db.Todos.Add(todo);
            _db.SaveChanges();

            return CreatedAtAction(
                nameof(GetTodoById),
                new { id = todo.TodoId },
                todo
            );
        }

        [HttpPut("{id}")]
        [TypeFilter(typeof(Todo_ValidateTodoIdFilterAttribute))]
        [Todo_ValidateUpdateTodoFilter]
        public IActionResult UpdateTodo(int id, Todo todo)
        {
            var todoToUpdate = HttpContext.Items["todo"] as Todo;

            todoToUpdate!.Title = todo.Title;
            todoToUpdate.IsCompleted = todo.IsCompleted;

            _db.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{id}")]
        [TypeFilter(typeof(Todo_ValidateTodoIdFilterAttribute))]
        public IActionResult DeleteTodo(int id)
        {
            var todoToDelete = HttpContext.Items["todo"] as Todo;

            _db.Todos.Remove(todoToDelete!);
            _db.SaveChanges();

            return Ok(todoToDelete);
        }
    }
}
