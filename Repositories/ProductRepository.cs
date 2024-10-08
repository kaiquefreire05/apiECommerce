﻿using ECommerceApi.Database;
using ECommerceApi.Enums;
using ECommerceApi.Models;
using ECommerceApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        // Injecting Database Dependecy
        private readonly ECommerceDBContext _context;
        public ProductRepository(ECommerceDBContext context)
        {
            _context = context;
        }

        // Methods
        public async Task<ProductModel> CreateProduct(ProductModel product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            ProductModel productModel = await GetProductById(id);
            if (productModel == null)
            {
                throw new Exception($"The product with ID: {id} is not founded in database.");
            }
            _context.Products.Remove(productModel);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ProductModel>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<ProductModel> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<ProductModel>> GetProductsByCategory(CategoriesEnum category)
        {
            return await _context.Products.Where(p => p.Category == category).ToListAsync();
        }

        public async Task<ProductModel> UpdateProduct(int prodId, ProductModel product)
        {
            var productFind = await GetProductById(prodId);
            if (productFind == null)
            {
                throw new Exception($"The product with ID: {prodId} is not founded in database.");
            }

            productFind.Name = product.Name;
            productFind.Description = product.Description;
            productFind.Price = product.Price;
            productFind.Stock = product.Stock;
            productFind.Category = product.Category;

            _context.Products.Update(productFind);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
