﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace NetCoreManager.Mvc.Controllers
{
    public class SharedController : BaseController
    {
        // GET: /<controller>/
        public IActionResult Error()
        {
            return View();
        }

        public IActionResult PageNoFound()
        {
            return View();
        }
    }
}
