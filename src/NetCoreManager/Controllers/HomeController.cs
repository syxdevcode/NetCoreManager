using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCoreManager.Application.Interface;

namespace NetCoreManager.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public readonly IUserService _IUserService;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="iUserService"></param>
        public HomeController(IUserService iUserService)
        {
            _IUserService = iUserService;
        }

        public async Task<IActionResult> Index()
        {
           var user=await _IUserService.GetById(Guid.NewGuid());
            return View(user);
        }
    }
}
