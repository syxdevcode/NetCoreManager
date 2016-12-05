using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreManager.Application.Interface;

namespace NetCoreManager.WebApi.Controllers
{
    [Route("login")]
    public class LoginController:Controller
    {
        

        public LoginController(IUserService userService)
        {
            
        }

        [Route("index")]
        [HttpGet]

        public IActionResult Index()
        {
            return View();
        }
    }
}
