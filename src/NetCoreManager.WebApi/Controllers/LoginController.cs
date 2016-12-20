using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreManager.Application.Interface;
using NetCoreManager.WebApi.Filter;

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
        public string Index()
        {
            return "Test";
        }
    }
}
