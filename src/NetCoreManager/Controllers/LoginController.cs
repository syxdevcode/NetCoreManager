using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreManager.Application.Interface;
using NetCoreManager.Component.Tools.Convert;
using NetCoreManager.Mvc.Model;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreManager.Mvc.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;

        public LoginController(IUserService userService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            _userService = userService;
        }


        [AllowAnonymous]
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                //检查用户信息
                var user = await _userService.Login(model.Account.ToLower(), model.Password);
                if (user!=null)
                {
                    //记录Session
                    HttpContext.Session.Set("_CurrentUser", ByteConvertHelper.Object2Bytes(user));

                    //跳转到系统首页
                    return RedirectToAction("Index", "Home");
                }
                ViewBag.ErrorInfo = "用户名或密码错误。";
                return View();
            }

            foreach (var item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    ViewBag.ErrorInfo = item.Errors[0].ErrorMessage;
                    break;
                }
            }
            return View(model);
        }

        public IActionResult SignOut()
        {
            HttpContext.Session.Remove("_CurrentUser");

            //跳转到系统首页
            return RedirectToAction("Index", "Login");
        }
    }
}
