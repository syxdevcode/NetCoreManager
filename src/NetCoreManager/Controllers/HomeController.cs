using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreManager.Application.Interface;
using NetCoreManager.Mvc.Filter;

namespace NetCoreManager.Mvc.Controllers
{
    [ServiceFilter(typeof(LoginActionFilter))]
    public class HomeController : BaseController
    {
        private readonly IUserService UserService;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="userService"></param>
        public HomeController(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            UserService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var id = new Guid("a2876802-e7f5-4ebf-9817-6b21541e6e36");
            var user = await UserService.GetById(id);
            return View(user);
        }
    }
}
