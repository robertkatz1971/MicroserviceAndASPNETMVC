using System.Security.Claims;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        public AuthController(IAuthService authService, ITokenProvider token)
        {
            _authService = authService;
            _tokenProvider = token;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginRequestDto());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            var loginResponse = await _authService.LoginAsync(model);

            if (loginResponse != null && loginResponse.IsSuccess)
            {
                TempData["success"] = "Logged in successfully";
                var loginResponseDto = JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(loginResponse.Result) + string.Empty);

                if (loginResponseDto != null)
                {
                    await SignInUser(loginResponseDto);
                    _tokenProvider.SetToken(loginResponseDto.Token + string.Empty);
                }
                   
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = loginResponse?.Message;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>()
            {
                new SelectListItem { Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin },
                new SelectListItem { Text=StaticDetails.RoleCustomer, Value=StaticDetails.RoleCustomer },
            };

            ViewBag.RoleList = roleList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegistrationRequestDto model)
        {
            var registrationResult = await _authService.RegisterAsync(model);

            if (registrationResult != null && registrationResult.IsSuccess)
            {
                //add role
                if (string.IsNullOrEmpty(model.Role))
                {
                    //default role
                    model.Role = StaticDetails.RoleCustomer;
                }
                var addRoleResult = await _authService.AssignRole(model);

                if (addRoleResult != null && addRoleResult.IsSuccess)
                {
                    TempData["success"] = "Registered successfully";
                    return RedirectToAction(nameof(Login));
                }
                
            }
            else
            {
                TempData["error"] = registrationResult?.Message;
            }

            var roleList = new List<SelectListItem>()
                {
                    new SelectListItem { Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin },
                    new SelectListItem { Text=StaticDetails.RoleCustomer, Value=StaticDetails.RoleCustomer },
                };

            ViewBag.RoleList = roleList;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(model.Token);
           
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
           
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Role,
                jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            
           
        }
    }
}

