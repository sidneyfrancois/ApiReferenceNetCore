using ReferenceAPI6.Models;
using ReferenceAPI6.Repository;

namespace ReferenceAPI6.Services;

public class ProductService
{
   private readonly ProductRepository productRepository = new();

   public Product GetProduct()
   {
      return productRepository.GetProduct();
   }
}