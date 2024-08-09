using AutoMapper;
using ECommerceApi.DTOs;
using ECommerceApi.Enums;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _rep;
        private readonly IMapper _map;

        public ProductController(IProductRepository rep, IMapper map)
        {
            _rep = rep;
            _map = map;
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productDto">The product data transfer object containing product details.</param>
        /// <returns>Returns the created product details.</returns>
        /// <response code="201">Returns the newly created product</response>
        /// <response code="400">If the product is null</response>
        [Authorize]
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
            return CreatedAtAction(nameof(GetProductById), new { id = createdProductDto.Id }, createdProductDto);
        }

        /// <summary>
        /// Deletes a product by ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>Returns the status of the deletion.</returns>
        /// <response code="200">If the product was successfully deleted</response>
        /// <response code="404">If the product was not found</response>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductDTO>> DeleteProduct(int id)
        {
            var delete = await _rep.DeleteProduct(id);
            return Ok(delete);
        }

        /// <summary>
        /// Retrieves all products.
        /// </summary>
        /// <returns>Returns a list of all products.</returns>
        /// <response code="200">Returns the list of products</response>
        [HttpGet]
        public async Task<ActionResult<List<ProductDTO>>> GetAllProducts()
        {
            var products = await _rep.GetAllProducts();
            return Ok(_map.Map<List<ProductDTO>>(products));
        }

        /// <summary>
        /// Retrieves a product by ID.
        /// </summary>
        /// <param name="id">The product ID.</param>
        /// <returns>Returns the product details.</returns>
        /// <response code="200">Returns the product details</response>
        /// <response code="404">If the product was not found</response>
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

        /// <summary>
        /// Retrieves products by category.
        /// </summary>
        /// <param name="category">The category of the products.</param>
        /// <returns>Returns a list of products in the specified category.</returns>
        /// <response code="200">Returns the list of products</response>
        [Authorize]
        [HttpGet("categories/{category}")]
        public async Task<ActionResult<List<ProductDTO>>> GetProductByCategory(CategoriesEnum category)
        {
            var products = await _rep.GetProductsByCategory(category);
            return Ok(_map.Map<List<ProductDTO>>(products));
        }

        /// <summary>
        /// Updates a product's information.
        /// </summary>
        /// <param name="productDto">The product data transfer object with updated information.</param>
        /// <param name="id">The product ID.</param>
        /// <returns>Returns the updated product details.</returns>
        /// <response code="200">If the product was successfully updated</response>
        /// <response code="404">If the product was not found</response>
        [Authorize]
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
