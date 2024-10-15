using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Models.DTO;
using ProductApi.Models.Requests;
using ProductApi.Models.Response;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductService _productService;

        public ProductsController(IMapper mapper, IProductService productService)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProducts([FromQuery] string name = "")
        {
            var products = await _productService.GetAllAsync(name);
            return Ok(ApiResponse<List<ProductDTO>>.Success(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(ApiResponse<List<ProductDTO>>.Fail("Product not found."));
            }

            return Ok(ApiResponse<ProductDTO>.Success(product));
        }

        [HttpPost]
        public async Task<ActionResult<ProductDTO>> AddProduct([FromBody] ProductCreateRequest createRequest)
        {
            var product = _mapper.Map<ProductDTO>(createRequest);
            await _productService.AddAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.ID }, ApiResponse<ProductDTO>.Success(product));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductUpdateRequest updatedProduct)
        {
            if (id != updatedProduct.ID)
            {
                return BadRequest(ApiResponse<string>.Fail("ID missmatch."));
            }

            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(ApiResponse<string>.Fail("Product not found."));
            }

            _mapper.Map(updatedProduct, product);
            await _productService.UpdateAsync(product);
            return Ok(ApiResponse<string>.Success("Product updated successfully."));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound(ApiResponse<string>.Fail("Product not found."));
            }

            await _productService.DeleteAsync(id);
            return Ok(ApiResponse<string>.Success("Product deleted successfully."));
        }

        [HttpPost("AddProductVersion")]
        public async Task<ActionResult<ProductVersionDTO>> AddProductVersion([FromBody] ProductVersionCreateRequest createRequest)
        {
            var productVersion = _mapper.Map<ProductVersionDTO>(createRequest);
            await _productService.AddAsync(productVersion);
            return CreatedAtAction(nameof(GetProduct), new { id = productVersion.ID }, ApiResponse<ProductVersionDTO>.Success(productVersion));
        }

        [HttpPut("UpdateProductVersion/{id}")]
        public async Task<IActionResult> UpdateProductVersion(Guid id, [FromBody] ProductVersionUpdateRequest updatedProduct)
        {
            if (id != updatedProduct.ID)
            {
                return BadRequest(ApiResponse<string>.Fail("Invalid Request."));
            }

            var productVersion = await _productService.GetVersionByIdAsync(id);

            if (productVersion == null)
            {
                return NotFound(ApiResponse<string>.Fail("Product Version not found."));
            }

            _mapper.Map(updatedProduct, productVersion);
            await _productService.UpdateAsync(productVersion);
            return Ok(ApiResponse<string>.Success("Product Version updated successfully."));
        }

        [HttpDelete("DeleteProductVersion/{id}")]
        public async Task<IActionResult> DeleteProductVersion(Guid id)
        {
            var productVersion = await _productService.GetVersionByIdAsync(id);

            if (productVersion == null)
            {
                return NotFound(ApiResponse<string>.Fail("Product Version not found."));
            }

            await _productService.DeleteAsync(id);
            return Ok(ApiResponse<string>.Success("Product Version deleted successfully."));
        }
    }
}
