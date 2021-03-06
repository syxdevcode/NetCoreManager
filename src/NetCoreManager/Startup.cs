﻿using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCoreManager.Infrastructure;
using NetCoreManager.Infrastructure.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using NetCoreManager.Component.Tools.Service;
using NetCoreManager.Mvc.Filter;
using NetCoreManager.Infrastructure.UnitOfWork;
using NetCoreManager.Component.Tools.Middleware;
using Serilog;

namespace NetCoreManager.Mvc
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"hosting.json", optional: true)
                .AddEnvironmentVariables()
                .AddUserSecrets<Startup>();
            Configuration = builder.Build();
        }

        //public IConfiguration Configuration { get; }

        public static IConfigurationRoot Configuration { get; set; }

        public IContainer ApplicationContainer { get; private set; }

        //IServiceProvider
        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Transient 服务在每次请求时被创建，它最好被用于轻量级无状态服务（如我们的Repository和ApplicationService服务）
            //services.AddTransient<IApplicationService, ApplicationService>

            //Scoped 服务在每次请求时被创建，生命周期横贯整次请求
            //services.AddScoped<IApplicationService, ApplicationService>

            //顾名思义，Singleton（单例） 服务在第一次请求时被创建（或者当我们在ConfigureServices中指定创建某一实例并运行方法），
            //其后的每次请求将沿用已创建服务。如果开发者的应用需要单例服务情景，请设计成允许服务容器来对服务生命周期进行操作，
            //而不是手动实现单例设计模式然后由开发者在自定义类中进行操作
            //services.AddSingleton<IApplicationService, ApplicationService>


            //获取数据库连接字符串
            var sqlConnectionString = Configuration["ConnectionStrings:Default"];

            //添加数据上下文
            services.AddDbContext<ManagerDbContext>(options => options.UseNpgsql(sqlConnectionString));

            //注入DbContext
            services.AddScoped<IUnitOfWork<ManagerDbContext>, UnitOfWork<ManagerDbContext>>();

            //注入获取application配置帮助类
            services.AddTransient<ApplicationConfigurationService>();

            services.Configure<ApplicationConfiguration>(Configuration.GetSection("ApplicationConfiguration"));

            #region 测试路由前缀

            // Add framework services.
            // 添加路由前缀
            //services.AddMvc(opt =>
            //{
            //    opt.UseCentralRoutePrefix(new RouteAttribute("api/{version}"));
            //});

            #endregion

            services.AddMvc();

            //Session服务
            services.AddSession();

            // 添加对输出的内容进行压缩组件：Gzip
            services.AddResponseCompression();

            //添加缓存
            services.AddResponseCaching();

            //登录拦截服务
            services.AddScoped<LoginActionFilter>();

            //添加elm
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

            var builder = new ContainerBuilder();
            builder.RegisterModule(new AutofacModule());
            builder.Populate(services);
            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //添加seq日志
            //loggerFactory.AddSeq(Configuration.GetSection("Seq"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Shared/Error");
            }

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    const int durationInSeconds = 60 * 60 * 24;
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + durationInSeconds;
                }
            });

            //添加中间件防止图片盗链
            app.UseHotlinkingPreventionMiddleware();

            //Session
            app.UseSession();

            // 添加对输出的内容进行压缩组件：Gzip
            app.UseResponseCompression();

            //添加缓存
            app.UseResponseCaching();

            #region elm
            loggerFactory.AddSerilog();

            // Ensure any buffered events are sent at shutdown
            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);
            #endregion


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            // If you want to dispose of resources that have been resolved in the
            // application container, register for the "ApplicationStopped" event.
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());


            SeedData.Initialize(app.ApplicationServices); //初始化数据

        }
    }
}
