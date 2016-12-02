using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace NetCoreManager.Component.Tools.Middleware
{
    public static class BuilderExtensions
    {
        public static IApplicationBuilder UseHotlinkingPreventionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<HotlinkingPreventionMiddleware>();
        }
    }

    /// <summary>
    /// 防止图片盗链中间件
    /// </summary>
    public class HotlinkingPreventionMiddleware
    {
        private readonly string _wwwrotFolder;

        private readonly RequestDelegate _next;

        public HotlinkingPreventionMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            _wwwrotFolder = env.WebRootPath;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var applicationUrl = $"{context.Request.Scheme}://{context.Request.Host.Value}";

            var headerDictionary = context.Request.Headers;

            var urlReferrer = headerDictionary[HeaderNames.Referer].ToString();

            if (!string.IsNullOrEmpty(urlReferrer) && !urlReferrer.StartsWith(applicationUrl))
            {
                var unauthorizzedImagePath = Path.Combine(_wwwrotFolder, "Images/Unauthorized.jpg");

                await context.Response.SendFileAsync(unauthorizzedImagePath);
            }

            await _next(context);
        }
    }
}
