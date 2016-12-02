using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreManager.Application.Interface;

namespace NetCoreManager.Mvc.Controllers
{
    [Route("test")]
    public class TestController:BaseController
    {
        public readonly IUserService UserService;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="userService"></param>
        public TestController(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            UserService = userService;
        }

        [Route("index/{id}")]
        public async Task<string> Index(string id,int version)
        {
            var idd = new Guid("cde2389c-de0d-4e57-8a8c-f42f92c474e6");
            var user = await UserService.GetById(idd);
            return user.Account;
        }
    }
}
