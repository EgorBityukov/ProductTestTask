using ProductApi.Models.DTO;

namespace ProductApi.Services
{
    public interface IProductService
    {
        Task<List<ProductDTO>> GetAllAsync(string name);
        Task<ProductDTO> GetByIdAsync(Guid id);
        Task<ProductVersionDTO> GetVersionByIdAsync(Guid id);
        Task AddAsync(ProductDTO productDto);
        Task UpdateAsync(ProductDTO productDto);
        Task DeleteAsync(Guid id);
        Task AddAsync(ProductVersionDTO productDto);
        Task UpdateAsync(ProductVersionDTO productDto);
        Task DeleteProductVersionAsync(Guid id);
    }
}
