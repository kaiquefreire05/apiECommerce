using AutoMapper;
using ECommerceApi.DTOs;
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
        // Injecting database methods and mapping
        private readonly IProductRepository _rep;
        private readonly IMapper _map;
        public ProductController(IProductRepository rep, IMapper map)
        {
            _rep = rep;
            _map = map;
        }

        // API Methods
        [HttpPost]
        public async Task<ActionResult<ProductDTO>> CreateProduct([FromBody] ProductDTO productDto)
        {
            if (productDto == null)
            {
                return BadRequest("Product cannot be null.");
            }
            var product = _map.Map<ProductModel>(productDto);
            var createdProduct = await _rep.CreateProduct(product);
            var createdProductDto = _map.Map<ProductDTO>(createdProduct);
            return CreatedAtAction(nameof(GetProductById), new {id = createdProductDto.Id}, createdProductDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDTO>> DeleteProduct(int id)
        {
            var delete = await _rep.DeleteProduct(id);
            return Ok(delete);
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            var products = await _rep.GetAllProducts();
            return Ok(_map.Map<List<ProductDTO>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProductById(int id)
        {
            var findProduct = await _rep.GetProductById(id);
            if (findProduct == null)
            {
                return NotFound();
            }
            return Ok(_map.Map<ProductDTO>(findProduct)); 
        }

        [HttpGet("categories/{category}")]
        public async Task<ActionResult<List<ProductDTO>>> GetProductByCategory(CategoriesEnum category)
        {
            var products = await _rep.GetProductsByCategory(category);
            return Ok(_map.Map<List<ProductDTO>>(products));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProductDTO>> UpdateProduct([FromBody] ProductDTO productDto, int id)
        {
            try
            {
                var updatedProduct = await _rep.UpdateProduct(id, _map.Map<ProductModel>(productDto));
                return Ok(_map.Map<ProductDTO>(updatedProduct));  
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }
    }
}
