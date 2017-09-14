using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore;

namespace NetCoreManager.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                       .MinimumLevel.Debug()
                       .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                       .Enrich.FromLogContext()
                       .WriteTo.Console()
                       .CreateLogger();
            try
            {
                Log.Information("Getting the motors running...");
                var host = new WebHostBuilder()
                .UseKestrel()
                //.UseUrls("http://localhost:5000") 已经配置hosting.json
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseSerilog()
                .UseStartup<Startup>()
                .Build();

                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly"); 
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
