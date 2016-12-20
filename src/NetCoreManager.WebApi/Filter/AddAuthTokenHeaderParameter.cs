using Microsoft.AspNetCore.Authorization;
using Swashbuckle.Swagger.Model;
using Swashbuckle.SwaggerGen.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCoreManager.WebApi.Filter
{
    /// <summary>
    /// Swagger在Header中设置认证参数
    /// </summary>
    public class AddAuthTokenHeaderParameter: IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();
            var attrs = context.ApiDescription.ActionAttributes();

            foreach (var attr in attrs)
            {
                // 如果 Attribute 是我们自定义的验证过滤器
                if (attr.GetType() == typeof(AuthorizeAttribute))
                {
                    operation.Parameters.Add(new NonBodyParameter()
                    {
                        Name = "AuthToken",
                        In = "header",
                        Type = "string",
                        Required = false
                    });
                }
            }
        }
    }
}
