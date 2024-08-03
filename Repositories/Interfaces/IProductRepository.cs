using ECommerceApi.Enums;
using ECommerceApi.Models;

namespace ECommerceApi.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductModel> CreateProduct(ProductModel product);
        Task<List<ProductModel>> GetAllProducts();
        Task<ProductModel> GetProductById(int id);
        Task<List<ProductModel>> GetProductsByCategory(CategoriesEnum category);
        Task<ProductModel> UpdateProduct(int prodId, ProductModel product);
        Task<bool> DeleteProduct(int id);
    }
}
