using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Filters.ActionFilters
{
    public class Todo_ValidateCreateTodoFilterAttribute : ActionFilterAttribute
    {
        private readonly AppDbContext _db;
        public Todo_ValidateCreateTodoFilterAttribute(AppDbContext db)
        {
            _db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var todo = context.ActionArguments["todo"] as Todo;

            if(todo == null) // Se Todo não foi passado na requisição
            {
                context.ModelState.AddModelError("Todo", "Todo não pode ser nulo, deve ser enviado.");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }
            else // Todo foi passado, então verifica se já existe um Todo igual cadastrado (mesma tarefa e estado).
            {
                var existingTodo = _db.Todos.FirstOrDefault(
                    x => x.Title!.ToLower() == todo.Title!.ToLower() &&
                         x.IsCompleted == todo.IsCompleted
                );

                if (existingTodo != null) // Encontrou um todo igual? Então manda o erro e um BadRequest
                {
                    context.ModelState.AddModelError("Todo", "Todo com o mesmo título e estado já existe.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
            }
        }
    }
}
