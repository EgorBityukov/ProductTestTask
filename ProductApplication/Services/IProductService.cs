using ProductApplication.Models;

namespace ProductApplication.Services
{
    public interface IProductService
    {
        Task<List<Product>?> GetProductsAsync(string filter = "");
        Task<Product?> GetProductAsync(Guid id);
        Task<string?> CreateProductAsync(Product product);
        Task<string?> UpdateProductAsync(Guid id, Product product);
        Task<string?> DeleteProductAsync(Guid id);
    }
}
