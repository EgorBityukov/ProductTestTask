using AutoMapper;
using ProductApi.Data.Models;
using ProductApi.Data.Repository;
using ProductApi.Models.DTO;

namespace ProductApi.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<List<ProductDTO>> GetAllAsync(string? name = "", string? versionName = "", double? minVolume = null, double? maxVolume = null)
        {
            var productVersions = await _productRepository.GetProductsAsync(name, versionName, minVolume, maxVolume);

            var products = productVersions
                .GroupBy(s => new { s.ID, s.ProductName, s.ProductDescription })
                .Select(g => new ProductDTO
                {
                    ID = g.Key.ID,
                    Name = g.Key.ProductName,
                    Description = g.Key.ProductDescription,
                    ProductVersions = g.Select(pv => new ProductVersionDTO
                    {
                        ID = pv.ProductVersionID,
                        Name = pv.ProductVersionName,
                        Description = pv.ProductVersionDescription,
                        CreatingDate = pv.CreatingDate,
                        ProductID = pv.ProductID,
                        Height = pv.Height,
                        Width = pv.Width,
                        Length = pv.Length
                    }).ToList()
                })
                .ToList();

            return products;
        }

        public async Task<ProductDTO> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task AddAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                await _productRepository.DeleteAsync(product);
            }
        }

        public async Task<ProductVersionDTO> GetVersionByIdAsync(Guid id)
        {
            var product = await _productRepository.GetVersionByIdAsync(id);
            return _mapper.Map<ProductVersionDTO>(product);
        }

        public async Task AddAsync(ProductVersionDTO productDto)
        {
            var product = _mapper.Map<ProductVersion>(productDto);
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(ProductVersionDTO productDto)
        {
            var product = _mapper.Map<ProductVersion>(productDto);
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteProductVersionAsync(Guid id)
        {
            var product = await _productRepository.GetVersionByIdAsync(id);
            if (product != null)
            {
                await _productRepository.DeleteAsync(product);
            }
        }
    }
}
