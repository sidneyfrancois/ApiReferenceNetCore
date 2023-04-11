using Microsoft.AspNetCore.Mvc;
using ReferenceAPI.Data;

namespace ReferenceAPI.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("categories")]
        public IActionResult Get(
            [FromServices] BlogDataContext context
            )
        {
            return Ok(context.Categories.ToList());
        }
    }
}
