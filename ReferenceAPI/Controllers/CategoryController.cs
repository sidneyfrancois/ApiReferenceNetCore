﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReferenceAPI.Data;
using ReferenceAPI.DTO;
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
                return Ok(categories);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Falha interna no servidor: " + e);
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
                    return NotFound();
                }

                return Ok(category);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Falha interna no servidor: " + e);
            }
        }

        [HttpPost("categories")]
        public async Task<IActionResult> PostAsync(
            [FromServices] BlogDataContext context,
            [FromBody] CreateCategoryDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
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

                return Created($"v1/categories/{category.Id}", category);
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, "Nao foi possivel incluir a categoria: ");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Falha interna no servidor: " + e);
            }
        }

        [HttpPut("categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id,
            [FromBody] EditCategoryDTO model)
        {
            try
            {
                var category = await context
                .Categories
                .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                {
                    return NotFound();
                }

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(model);
            }
            catch (DbUpdateException e)
            {
                return BadRequest("Nao foi possivel atualizar a categoria: ");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Falha interna no servidor: " + e);
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
                    return NotFound();
                }

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(category);
            }
            catch (DbUpdateException e)
            {
                return BadRequest("Nao foi possivel deletar a categoria: ");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Falha interna no servidor: " + e);
            }
        }
    }
}
