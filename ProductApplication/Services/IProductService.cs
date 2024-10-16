using ProductApplication.Models;

namespace ProductApplication.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync(string filter = "");
    }
}
