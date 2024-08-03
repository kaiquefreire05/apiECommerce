using ECommerceApi.Enums;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        // Injecting database methods
        private readonly IProductRepository _rep;
        public ProductController(IProductRepository rep)
        {
            _rep = rep;
        }

        // API Methods
        [HttpPost]
        public async Task<ActionResult<ProductModel>> CreateProduct([FromBody] ProductModel product)
        {
            await _rep.CreateProduct(product);
            return CreatedAtAction(nameof(GetProductById), new {id = product.Id}, product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductModel>> DeleteProduct(int id)
        {
            var delete = await _rep.DeleteProduct(id);
            return Ok(delete);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductModel>>> GetAllProducts()
        {
            return Ok(await _rep.GetAllProducts());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProductById(int id)
        {
            var findProduct = await _rep.GetProductById(id);
            if (findProduct == null)
            {
                return NotFound();
            }
            return Ok(findProduct); 
        }

        [HttpGet("categories/{category}")]
        public async Task<ActionResult<List<ProductModel>>> GetProductByCategory(CategoriesEnum category)
        {
            return Ok(await _rep.GetProductsByCategory(category));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductModel>> UpdateProduct([FromBody] ProductModel product, int id)
        {
            try
            {
                var updatedProduct = await _rep.UpdateProduct(id, product);
                return Ok(updatedProduct);  
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
