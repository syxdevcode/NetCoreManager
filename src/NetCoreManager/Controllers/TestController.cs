using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreManager.Application.Interface;
using Microsoft.Extensions.Logging;

namespace NetCoreManager.Mvc.Controllers
{
    public class TestController : BaseController
    {
        private readonly IUserService UserService;

        private readonly ILogger<TestController> _log;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="userService"></param>
        public TestController(IUserService userService, ILogger<TestController> log)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            UserService = userService;
            _log = log;
        }

        [HttpGet]
        public async Task<string> Index()
        {
            //_log.LogInformation("Hello, world!");
            var idd = new Guid("cde2389c-de0d-4e57-8a8c-f42f92c474e6");
            var user = await UserService.GetById(idd);
            return user.Account;
        }
    }
}
