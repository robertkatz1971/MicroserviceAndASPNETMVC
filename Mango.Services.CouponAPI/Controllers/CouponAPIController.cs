using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Models;
using Mango.Services.CouponAPI.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/coupon")]
    [ApiController]
    public class CouponAPIController : ControllerBase
    {
        private readonly AppDBContext _context;
        private ResponseDto _responseDto;
        private readonly IMapper _mapper;

        public CouponAPIController(AppDBContext context, IMapper mapper)
        {
            _context = context;
            _responseDto = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseDto>> GetAllCoupons()
        {
            try
            {
                IEnumerable<Coupon> result = await _context.Coupons.ToListAsync();
                _responseDto.Result = _mapper.Map<IEnumerable<CouponDto>>(result);

            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpGet("id:int")]
        public async Task<ActionResult<ResponseDto>> GetCoupon(int id)
        {
            try
            {
                Coupon result = await _context.Coupons.FirstAsync(u => u.CouponId == id);
                _responseDto.Result = _mapper.Map<CouponDto>(result);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpGet("GetByCode/{code}")]
        public async Task<ActionResult<ResponseDto>> GetCouponByCode(string code)
        {
            try
            {
                Coupon? result = await _context.Coupons.FirstOrDefaultAsync(u => u.CouponCode == code);
                _responseDto.IsSuccess = result == null ? false : true;
                _responseDto.Result = _mapper.Map<CouponDto>(result);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPost]
        public ResponseDto AddCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon newCoupon = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Add(newCoupon);
                _context.SaveChanges();

                _responseDto.Result = _mapper.Map<CouponDto>(newCoupon);
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;
            }

            return _responseDto;
        }

        [HttpPut]
        public ResponseDto UpdateCoupon([FromBody] CouponDto couponDto)
        {
            try
            {
                Coupon coupon = _mapper.Map<Coupon>(couponDto);
                _context.Coupons.Update(coupon);
                _context.SaveChanges();

                _responseDto.Result = _mapper.Map<CouponDto>(coupon);
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
        public ResponseDto DeleteCoupon(int id)
        {
            try
            {
                
                _context.Coupons.Remove(_context.Coupons.Single(c => c.CouponId == id));
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


