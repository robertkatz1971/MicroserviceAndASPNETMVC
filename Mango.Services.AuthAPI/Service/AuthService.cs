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
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(AppDBContext appDBContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IJwtTokenGenerator jwtTokenGenerator)
		{
            _appDbContext = appDBContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
		}

        public async Task<bool> AssignRoles(string email, string roleName)
        {
            var user = _appDbContext.ApplicationUsers.SingleOrDefault(u => u.Email.ToLower() == email.ToLower());

            if(user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create roles
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _appDbContext.ApplicationUsers.SingleOrDefault(u => u.UserName.ToLower()  == loginRequestDto.UserName.ToLower());

            bool isValid = (user != null && await _userManager.CheckPasswordAsync(user, loginRequestDto.Password));

            if (isValid == false)
            {
                return new LoginResponseDto()
                {
                    User = null,
                    Token = string.Empty
                };
            }

            //if user was found, generate JWT token
            var token = _jwtTokenGenerator.GenerateToken(user);

            UserDto userDto = new()
            {
                Email = user?.Email,
                Id = user?.Id,
                Name = user?.Name,
                PhoneNumber = user?.PhoneNumber
            };

            return new LoginResponseDto()
            {
                User = userDto,
                Token = token
            };

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

