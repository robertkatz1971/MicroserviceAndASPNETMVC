using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Logout()
        {
            return View();
        }
    }
}

