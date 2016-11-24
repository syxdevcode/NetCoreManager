using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NetCoreManager.Mvc.Filter
{
    public class LoginActionFilter:IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //Todo
        }

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.Controller.GetType().Name;

            byte[] result;
            filterContext.HttpContext.Session.TryGetValue("_CurrentUser", out result);
            if (result == null)
            {
                filterContext.Result = new RedirectResult("/Login/Index");
            }
        }
    }
}
