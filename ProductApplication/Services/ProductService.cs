using ProductApplication.Models;

namespace ProductApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string filter = "")
        {
            var response = await _httpClient.GetAsync($"api/products?name={filter}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<IEnumerable<Product>>();
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"api/products/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsAsync<Product>();
        }

        public async Task CreateProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("api/products", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateProductAsync(Guid id, Product product)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/products/{id}", product);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/products/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
