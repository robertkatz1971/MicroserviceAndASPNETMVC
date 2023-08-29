using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: /<controller>/
        public async Task<IActionResult> ProductIndex()
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

        public IActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.CreateProductAsync(model); 

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Coupon added successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }

            }

            return View(model);
        }

        public IActionResult ProductEdit()
        {
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? response = await _productService.UpdateProductAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }

            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(int id)
        {
            ResponseDto? response = await _productService.DeleteProductAsync(id);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon deleted successfully";
                
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return RedirectToAction(nameof(ProductIndex));
        }
    }
}

