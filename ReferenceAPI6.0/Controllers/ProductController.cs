using Microsoft.AspNetCore.Mvc;
using ReferenceAPI6.Services;

namespace ReferenceAPI6;

[ApiController]
[Route("product")]
public class ProductController : ControllerBase
{
   private readonly ProductService productService = new();

   [HttpGet]
   public IActionResult GetProduct()
   {
      var product = productService.GetProduct();

      return Ok(product);
   }
}