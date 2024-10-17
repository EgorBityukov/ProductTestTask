using ProductApi.Data.Models;

namespace ProductApi.Data.Repository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductVersionsDAO>> GetProductsAsync(string? name = "", string? versionName = "", double? minVolume = null, double? maxVolume = null);
        Task<Product> GetByIdAsync(Guid id);
        Task<bool> isExistAsync(string name);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task DeleteAsync(Product product);
        Task<ProductVersion> GetVersionByIdAsync(Guid id);
        Task AddAsync(ProductVersion product);
        Task UpdateAsync(ProductVersion product);
        Task DeleteAsync(ProductVersion product);
    }
}
