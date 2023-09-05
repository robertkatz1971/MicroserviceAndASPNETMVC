using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Mango.Web.Models;
using Newtonsoft.Json;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;

namespace Mango.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProductService _productService;

    public HomeController(IProductService productService)
    {
        _productService = productService;
    }

    // GET: /<controller>/
    public async Task<IActionResult> Index()
    {
        List<ProductDto>? products = new();

        var response = await _productService.GetAllProductsAsync();

        if (response != null && response.IsSuccess)
        {
            products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result + ""));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return View(products);

    }

    public async Task<IActionResult> ProductDetails(int productId)
    {
        ProductDto? product = new();

        var response = await _productService.GetProductByIdAsync(productId);

        if (response != null && response.IsSuccess)
        {
            product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result + ""));
        }
        else
        {
            TempData["error"] = response?.Message;
        }

        return View(product);

    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

