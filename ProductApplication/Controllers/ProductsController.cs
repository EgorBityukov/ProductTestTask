using Microsoft.AspNetCore.Mvc;
using ProductApplication.Models;
using ProductApplication.Services;

public class ProductsController : Controller
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<IActionResult> Index(string filter = "")
    {
        var products = await _productService.GetProductsAsync(filter);
        return View(products);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product product)
    {
        if (ModelState.IsValid)
        {
            await _productService.CreateProductAsync(product);
            return RedirectToAction("Index");
        }
        return View(product);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var product = await _productService.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, Product product)
    {
        if (ModelState.IsValid)
        {
            await _productService.UpdateProductAsync(id, product);
            return RedirectToAction("Index");
        }
        return View(product);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var product = await _productService.GetProductAsync(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _productService.DeleteProductAsync(id);
        return RedirectToAction("Index");
    }
}