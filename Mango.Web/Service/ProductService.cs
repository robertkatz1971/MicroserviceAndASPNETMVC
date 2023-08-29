using System;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Service
{
    public class ProductService : IProductService
	{
        private IBaseService _baseService;

        public ProductService(IBaseService baseService)
		{
            _baseService = baseService;
		}

        public async Task<ResponseDto?> CreateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.ProductAPIBase,
                Data = productDto
            });
        }

        public async Task<ResponseDto?> DeleteProductAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = StaticDetails.ProductAPIBase + id.ToString(),
            });
        }

        public async Task<ResponseDto?> GetAllProductsAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = StaticDetails.ProductAPIBase
            });
        }

        public async Task<ResponseDto?> GetProductByNameAsync(string productName)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = StaticDetails.ProductAPIBase + "GetByName/" + productName
            });
        }

        public async Task<ResponseDto?> GetProductByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = StaticDetails.ProductAPIBase + id.ToString()
            }) ;
        }

        public async Task<ResponseDto?> UpdateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Url = StaticDetails.ProductAPIBase,
                Data = productDto
            });
        }
    }
}

