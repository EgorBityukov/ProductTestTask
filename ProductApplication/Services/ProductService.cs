using ProductApplication.Models;

namespace ProductApplication.Services
{
    public class ProductService : IProductService
    {
        private readonly ApiClient _apiClient;

        public ProductService(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<List<Product>?> GetProductsAsync(string filter = "")
        {
            var response = await _apiClient.GetAsync($"api/products?name={filter}");
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<Product>>>();

            if (apiResponse == null)
            {
                return null;
            }
            else
            if (!apiResponse.IsSuccess)
            {
                throw new Exception(apiResponse.ErrorMessage);
            }

            return apiResponse.Data;
        }

        public async Task<Product?> GetProductAsync(Guid id)
        {
            var response = await _apiClient.GetAsync($"api/products/{id}");
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<Product>>();

            if (apiResponse == null)
            {
                return null;
            }
            else
            if (!apiResponse.IsSuccess)
            {
                throw new Exception(apiResponse.ErrorMessage);
            }

            return apiResponse.Data;
        }

        public async Task<string?> CreateProductAsync(Product product)
        {
            var response = await _apiClient.PostAsync("api/products/AddProduct", product);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

            if (apiResponse == null)
            {
                return null;
            }
            else
            if (!apiResponse.IsSuccess)
            {
                throw new Exception(apiResponse.ErrorMessage);
            }

            return apiResponse.Data;
        }

        public async Task<string?> UpdateProductAsync(Guid id, Product product)
        {
            var response = await _apiClient.PutAsync($"api/products/UpdateProduct/{id}", product);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

            if (apiResponse == null)
            {
                return null;
            }
            else
            if (!apiResponse.IsSuccess)
            {
                throw new Exception(apiResponse.ErrorMessage);
            }

            return apiResponse.Data;
        }

        public async Task<string?> DeleteProductAsync(Guid id)
        {
            var response = await _apiClient.DeleteAsync($"api/products/DeleteProduct/{id}");
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<string>>();

            if (apiResponse == null)
            {
                return null;
            }
            else
            if (!apiResponse.IsSuccess)
            {
                throw new Exception(apiResponse.ErrorMessage);
            }

            return apiResponse.Data;
        }
    }
}
