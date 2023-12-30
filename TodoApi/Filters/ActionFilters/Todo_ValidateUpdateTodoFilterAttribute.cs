using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Filters.ActionFilters
{
    public class Todo_ValidateUpdateTodoFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var id = context.ActionArguments["id"] as int?;
            var todo = context.ActionArguments["todo"] as Todo;

            if (id.HasValue && todo != null && id != todo.TodoId) // O Todo id da rota e do body são diferentes? Retorna um BadRequest com o problema.
            {
                context.ModelState.AddModelError("TodoId", "O ID da rota e TodoId são diferentes!");
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Status = StatusCodes.Status400BadRequest
                };
                context.Result = new BadRequestObjectResult(problemDetails);
            }

            // Verificar se o 
        }
    }
}
