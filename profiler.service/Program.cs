using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using Destructurama;

namespace profiler.service
{
    public static class WebHostServiceExtensions
    {
        public static void RunAsWorkflowAppAsService(this IWebHost host)
        {
            var webHostService = new WorkflowAppWebHostService(host);
            ServiceBase.Run(webHostService);
        }
    }

    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var isService = !(Debugger.IsAttached || args.Contains("--console"));

                var builder = CreateWebHostBuilder(args);

                if (isService)
                {
                    var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                    var pathToContentRoot = Path.GetDirectoryName(pathToExe);

                    builder.UseContentRoot(pathToContentRoot);

                    builder
                        .Build() 
                        .RunAsWorkflowAppAsService();
                }
                else
                    builder
                        .Build() 
                        .Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        
       
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

                    Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(config.Build())
                        .Destructure.JsonNetTypes()
                        .CreateLogger();
                })
                .UseSerilog()
                .UseStartup<Startup>();
        }
    }
}
