using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductApplication.Models;
using ProductApplication.Services;
using System.Diagnostics;

public class ProductsController : Controller
{
    private readonly IProductService _productService;
    private readonly IMapper _mapper;

    public ProductsController(IProductService productService, IMapper mapper)
    {
        _productService = productService;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index(string filter = "")
    {
        var products = await _productService.GetProductsAsync(filter);
        return View(products);
    }

    public IActionResult Create()
    {
        var model = new ProductViewModel();
        return PartialView("_ProductForm", model);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var product = await _productService.GetProductAsync(id);
        var model = _mapper.Map<ProductViewModel>(product);
        return PartialView("_ProductForm", model);
    }

    [HttpPost]
    public async Task<IActionResult> Save(ProductViewModel model)
    {
        var product = _mapper.Map<Product>(model);

        try
        {
            if (product.ID != Guid.Empty)
            {
                await _productService.UpdateProductAsync(product.ID, product);
            }
            else
            {
                await _productService.CreateProductAsync(product);
            }
        }
        catch (Exception ex)
        {
            return Json(new { isSuccess = false, errorMessage = ex.Message });
        }

        return Json(new { isSuccess = true });
    }

    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}