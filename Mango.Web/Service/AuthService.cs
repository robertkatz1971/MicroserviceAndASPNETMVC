using System;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using static Mango.Web.Utility.StaticDetails;

namespace Mango.Web.Service
{
	public class AuthService : IAuthService
	{
        private IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> AssignRole(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.AuthAPIBase + "assignRole",
                Data = registrationRequestDto
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.CouponAPIBase + "login",
                Data = loginRequestDto
            });
        }

        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Url = StaticDetails.AuthAPIBase + "register",
                Data = registrationRequestDto
            });
        }
    }
}

