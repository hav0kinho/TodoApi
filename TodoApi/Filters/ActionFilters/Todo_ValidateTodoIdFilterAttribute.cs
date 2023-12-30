using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TodoApi.Data;

namespace TodoApi.Filters.ActionFilters
{
    public class Todo_ValidateTodoIdFilterAttribute : ActionFilterAttribute
    {
        private readonly AppDbContext _db;

        public Todo_ValidateTodoIdFilterAttribute(AppDbContext db)
        {
            _db = db;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var todoId = context.ActionArguments["id"] as int?;
            if(todoId.HasValue) // Id foi passado
            {
                if(todoId.Value <= 0) // Id é menor que 0? Manda o problema da requisião e um BadRequest
                {
                    context.ModelState.AddModelError("TodoId", "TodoId é inválido, pois é menor ou igual a zero.");
                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Status = StatusCodes.Status400BadRequest
                    };
                    context.Result = new BadRequestObjectResult(problemDetails);
                }
                else // O id é maior ou igual que zero
                {
                    var todo = _db.Todos.Find(todoId.Value);
                    if(todo == null) // Não encontrou o Todo? Manda o problema na requisição e um NotFound 
                    {
                        context.ModelState.AddModelError("TodoId", "O Todo não existe.");
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Status = StatusCodes.Status404NotFound
                        };
                        context.Result = new NotFoundObjectResult(problemDetails);
                    }
                    else // O Todo existe, manda pro controller via HttpContext
                    {
                        context.HttpContext.Items["todo"] = todo;
                    }
                }
            }
        }
    }
}
