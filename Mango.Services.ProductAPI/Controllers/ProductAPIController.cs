using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    [Authorize]
    public class ProductAPIController : ControllerBase
    {
        private readonly AppDBContext _context;
        private ResponseDto _responseDto;
        private readonly IMapper _mapper;

        public ProductAPIController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAllProducts()
        {
            try
            {
                IEnumerable<Product> result = await _context.Products.ToListAsync();
                _responseDto.Result = _mapper.Map<IEnumerable<ProductDto>>(result);

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpGet("id:int")]
        public async Task<ActionResult<ResponseDto>> GetProduct(int id)
        {
            try
            {
                Product result = await _context.Products.FirstAsync(u => u.ProductId == id);
                _responseDto.Result = _mapper.Map<ProductDto>(result);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpGet("GetByName/{name}")]
        public async Task<ActionResult<ResponseDto>> GetProductByName(string name)
        {
            try
            {
                Product? result = await _context.Products.FirstOrDefaultAsync(u => u.Name == name);
                _responseDto.IsSuccess = result == null ? false : true;
                _responseDto.Result = _mapper.Map<ProductDto>(result);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto AddProduct([FromBody] ProductDto productDto)
        {
            try
            {
                Product newProduct = _mapper.Map<Product>(productDto);
                _context.Products.Add(newProduct);
                _context.SaveChanges();

                _responseDto.Result = _mapper.Map<ProductDto>(newProduct);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPut]
        public ResponseDto UpdateProduct([FromBody] ProductDto productDto)
        {
            try
            {
                Product coupon = _mapper.Map<Product>(productDto);
                _context.Products.Update(coupon);
                _context.SaveChanges();

                _responseDto.Result = _mapper.Map<ProductDto>(coupon);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto DeleteProduct(int id)
        {
            try
            {

                _context.Products.Remove(_context.Products.Single(c => c.ProductId == id));
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }
    }
}




