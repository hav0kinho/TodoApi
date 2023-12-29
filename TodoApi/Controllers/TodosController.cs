using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Data;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly AppDbContext db;

        public TodosController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult GetTodos()
        {
            return Ok("Retornando Todo's");
        }
    }
}
