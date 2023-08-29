using System;
using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
	public interface IProductService
	{
		Task<ResponseDto?> GetProductByIdAsync(int id);
        Task<ResponseDto?> GetProductByNameAsync(string productName);
        Task<ResponseDto?> GetAllProductsAsync();
        Task<ResponseDto?> CreateProductAsync(ProductDto productDto);
        Task<ResponseDto?> UpdateProductAsync(ProductDto productDto);
        Task<ResponseDto?> DeleteProductAsync(int id);
    }
}

