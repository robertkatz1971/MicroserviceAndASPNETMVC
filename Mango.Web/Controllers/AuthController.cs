using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("CustomerError", loginResponse?.Message + string.Empty);
            return View(model);
   
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
            var roleList = new List<SelectListItem>()
                {
                    new SelectListItem { Text=StaticDetails.RoleAdmin, Value=StaticDetails.RoleAdmin },
                    new SelectListItem { Text=StaticDetails.RoleCustomer, Value=StaticDetails.RoleCustomer },
                };

            ViewBag.RoleList = roleList;

            return View(model);
        }

        [HttpPost]
        public IActionResult Logout()
        {
            return View();
        }
    }
}

