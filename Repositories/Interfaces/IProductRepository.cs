using ECommerceApi.Enums;
using ECommerceApi.Models;

namespace ECommerceApi.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<ProductsModel> CreateProduct(ProductsModel product);
        Task<List<ProductsModel>> GetAllProducts();
        Task<ProductsModel> GetProductById(int id);
        Task<List<ProductsModel>> GetProductsByCategorie(CategoriesEnum category);
        Task<ProductsModel> UpdateProduct(int prodId, ProductsModel product);
        Task<bool> DeleteProduct(int id);
    }
}
