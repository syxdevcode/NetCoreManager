﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCoreManager.Application.Interface;
using NetCoreManager.Component.Tools.Service;
using NetCoreManager.Component.Tools.ConvertHelper;
using NetCoreManager.Component.Tools.Encrypt;
using NetCoreManager.Mvc.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreManager.Mvc.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ApplicationConfigurationService _applicationConfigurationService;
        private static readonly CryptoTool crypto = CryptoTool.Create(CryptoTypes.EncAes);

        public LoginController(IUserService userService, ApplicationConfigurationService applicationConfigurationService)
        {
            if (userService == null)
            {
                throw new ArgumentNullException(nameof(userService));
            }
            if (applicationConfigurationService == null)
            {
                throw new ArgumentNullException(nameof(applicationConfigurationService));
            }
            _userService = userService;
            _applicationConfigurationService = applicationConfigurationService;
        }

        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader = "Accept-Encoding", Location = ResponseCacheLocation.Any, Duration = 10)]
        public IActionResult Index(string returnUrl = null)
        {
            Response.GetTypedHeaders().CacheControl = new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromSeconds(10)
            };
            Response.Headers[HeaderNames.Vary] = new string[] { "Accept-Encoding" };
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var captchaString = this.Request.Form["__captcha_image"];
                var encryptedString = crypto.Encrypt(model.Captcha.ToLower(), "DotNetCore!");
                if (captchaString != encryptedString)
                {
                    //ModelState.AddModelError("", "验证码不正确。");
                    ViewBag.ErrorInfo = "验证码不正确。";
                    return View(nameof(Index));
                }

                model.Password = EncryptHelper.Encrypt(model.Password, _applicationConfigurationService.AppConfigurations.PwdSalt);

                //检查用户信息
                var user = await _userService.Login(model.Account.ToLower(), model.Password);
                if (user != null)
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
