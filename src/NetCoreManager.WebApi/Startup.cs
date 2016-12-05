using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreManager.Component.Tools.Service;
using NetCoreManager.Component.Tools.OptionsExtensions;
using NetCoreManager.Infrastructure;
using NetCoreManager.Infrastructure.IoC;
using NetCoreManager.Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using NetCoreManager.WebApi.Auth;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace NetCoreManager.WebApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public IContainer ApplicationContainer { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //获取数据库连接字符串
            var sqlConnectionString = Configuration.GetConnectionString("Default");

            //添加数据上下文
            services.AddDbContext<ManagerDbContext>(options => options.UseNpgsql(sqlConnectionString));

            //注入DbContext
            services.AddScoped<IUnitOfWork<ManagerDbContext>, UnitOfWork<ManagerDbContext>>();

            //注入获取application配置帮助类
            services.AddTransient<ApplicationConfigurationService>();

            services.Configure<ApplicationConfiguration>(Configuration.GetSection("ApplicationConfiguration"));

            // Enable the use of an [Authorize("Bearer")] attribute on methods and classes to protect.
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
            #region 添加资源跨越
            //services.AddCors();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowSpecificOrigin", builders =>
            //    {
            //        builders.WithOrigins("http://localhost", "http://localhost:9090");
            //    });
            //});
            //see the controller attribute
            //like [EnableCors("AllowSpecificOrigin")]
            #endregion

            #region 添加CORS支持通配符解决方案

            services.AddScoped<ICorsService, WildcardCorsService>();
            services.Configure<CorsOptions>(options => options.AddPolicy(
                "AllowSameDomain", builders => builders.WithOrigins("*.syxtest.com")));
            /*使用
             * [EnableCors("AllowSameDomain")]
            */
            #endregion

            #region 测试路由前缀
            //Add framework services.
            //添加路由前缀
            services.AddMvc(opt =>
            {
                opt.UseCentralRoutePrefix(new RouteAttribute("api/v{version}"));
            });
            #endregion
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule());
            builder.Populate(services);
            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            #region Handle Exception
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    //when authorization has failed, should retrun a json message to client
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(
                            new { authenticated = false, tokenExpired = true }
                        ));
                    }
                    //when orther error, retrun a error message json to client
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(
                            new { success = false, error = error.Error.Message }
                        ));
                    }
                    //when no error, do next.
                    else await next();
                });
            });
            #endregion

            #region UseJwtBearerAuthentication
            var options = new JwtBearerOptions();
            options.TokenValidationParameters.IssuerSigningKey = TokenAuthOption.Key;
            options.TokenValidationParameters.ValidAudience = TokenAuthOption.Audience;
            options.TokenValidationParameters.ValidIssuer = TokenAuthOption.Issuer;

            // When receiving a token, check that we've signed it.
            options.TokenValidationParameters.ValidateIssuerSigningKey = true;

            // When receiving a token, check that it is still valid.
            options.TokenValidationParameters.ValidateLifetime = true;

            // This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time 
            // when validating the lifetime. As we're creating the tokens locally and validating them on the same 
            // machines which should have synchronised time, this can be set to zero. Where external tokens are
            // used, some leeway here could be useful.
            options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(0);

            app.UseJwtBearerAuthentication(options);
            #endregion

            #region 添加资源跨越
            //app.UseCors("AllowSpecificOrigin");
            #endregion
            
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            app.UseMvc();
        }
    }
}
