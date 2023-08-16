using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _responseDto;

        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _responseDto = new();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var registrationResponse = await _authService.Register(model);

            if (!string.IsNullOrEmpty(registrationResponse))
            {
                //error
                _responseDto.IsSuccess = false;
                _responseDto.Message = registrationResponse;
                return BadRequest(_responseDto);
            }
            else
            {
                _responseDto.IsSuccess = true;
            }

            return Ok(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);

            if (loginResponse.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or password is not correct.";
                return BadRequest(_responseDto);
            }

            _responseDto.Result = loginResponse;
           
            return Ok(_responseDto);
        }
    }
}

