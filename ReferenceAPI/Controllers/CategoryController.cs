using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReferenceAPI.Data;
using ReferenceAPI.DTO;
using ReferenceAPI.ErrorViewModel;
using ReferenceAPI.Extensions;
using ReferenceAPI.Models;

namespace ReferenceAPI.Controllers
{
    [ApiController]
    [Route("v1")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("Falha interna no servidor"));
            }
        }

        [HttpGet("categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id)
        {
            try
            {
                var category = await context
                .Categories
                .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return NotFound(new ResultViewModel<Category>("categoria nao encontrada"));
                }

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (Exception e)
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }

        [HttpPost("categories")]
        public async Task<IActionResult> PostAsync(
            [FromServices] BlogDataContext context,
            [FromBody] CreateCategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            try
            {
                var category = new Category()
                {
                    Id = 0,
                    Posts = null,
                    Name = model.Name,
                    Slug = model.Slug.ToLower()
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResultViewModel<Category>("Nao foi possivel incluir a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Nao foi possivel incluir a categoria"));
            }
        }

        [HttpPut("categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id,
            [FromBody] EditCategoryDTO model)
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));
            }

            try
            {
                var category = await context
                .Categories
                .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return NotFound(new ResultViewModel<Category>("Categoria nao encontrada"));
                }

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException e)
            {
                return BadRequest(new ResultViewModel<Category>("Nao foi possivel atualizar a categoria"));
            }
            catch 
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }

            
        }

        [HttpDelete("categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id)
        {
            try
            {
                var category = await context
                .Categories
                .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return NotFound(new ResultViewModel<Category>("Categoria nao encontrada"));
                }

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(category);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new ResultViewModel<Category>("Nao foi possivel deletar a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }
    }
}
