using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace NetCoreManager.Component.Tools.OptionsExtensions
{
    public static class RoutePrefixExtensions
    {
        public static void UseCentralRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
        {
            //添加我们自定义实现IApplicationModelConvention的RouteConvention
            opts.Conventions.Insert(0,new RouteConvention(routeAttribute));
        }
    }

    /// <summary>
    /// 定义RoutConvention，实现IApplicationModelConvention接口
    /// </summary>
    public class RouteConvention : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _centerlPrefix;

        public RouteConvention(IRouteTemplateProvider routeTemplateProvider)
        {
            _centerlPrefix = new AttributeRouteModel(routeTemplateProvider);
        }

        /// <summary>
        /// 实现Apply方法
        /// </summary>
        /// <param name="application"></param>
        public void Apply(ApplicationModel application)
        {
            //遍历所有controller
            foreach (var controller in application.Controllers)
            {
                //已经标记了RouteAttribute的Controller
                var matchSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
                if (matchSelectors.Any())
                {
                    foreach (var selectorModel in matchSelectors)
                    {
                        //在当前路由上再添加 路由前缀
                        selectorModel.AttributeRouteModel=AttributeRouteModel.CombineAttributeRouteModel(_centerlPrefix,selectorModel.AttributeRouteModel);
                    }
                }

                //没有标记RoutAttribute的Controller
                var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
                if (unmatchedSelectors.Any())
                {
                    foreach (var selectorModel in unmatchedSelectors)
                    {
                        selectorModel.AttributeRouteModel = _centerlPrefix;
                    }
                }
            }
        }

    }
}
