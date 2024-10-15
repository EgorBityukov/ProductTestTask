using ProductApi.Data.Models;

namespace ProductApi.Data.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProductsByNameAsync(string name);
        Task<Product> GetByIdAsync(Guid id);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<ProductVersion> GetVersionByIdAsync(Guid id);
        Task AddAsync(ProductVersion product);
        Task UpdateAsync(ProductVersion product);
        Task DeleteAsync(ProductVersion product);
    }
}
