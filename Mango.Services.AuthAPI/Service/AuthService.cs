using System;
using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service
{
	public class AuthService : IAuthService
	{
        private readonly AppDBContext _appDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

		public AuthService(AppDBContext appDBContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
            _appDbContext = appDBContext;
            _userManager = userManager;
            _roleManager = roleManager;
		}

        public Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email?.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password + string.Empty);

                if (result.Succeeded)
                {
                    var userFromDB = _appDbContext.ApplicationUsers.Single(u => u.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email = userFromDB.Email + string.Empty,
                        Id = userFromDB.Id,
                        Name = userFromDB.Name + string.Empty,
                        PhoneNumber = userFromDB.PhoneNumber + string.Empty
                    };

                    return string.Empty;
                }
                else
                {
                    return result.Errors.First().Description;
                }


            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }
    }
}

