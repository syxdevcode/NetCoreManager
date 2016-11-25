using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreManager.Domain.Entity;
using NetCoreManager.Infrastructure;

namespace NetCoreManager.Test.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork<ManagerDbContext> _unitOfWork;

        public HomeController(IUnitOfWork<ManagerDbContext> unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }
            this._unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _unitOfWork.Repository<User>().FindAsync(new Guid("cde2389c-de0d-4e57-8a8c-f42f92c474e6"));

            return View(user);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
