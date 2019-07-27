using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace profiler.consumers
{
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
                    //var pathToContentRoot = Path.GetDirectoryName(pathToExe);

                    //builder.UseContentRoot(pathToContentRoot);

                    //var host = builder.Build();
                    //ServiceBase.Run(new WorkflowAppWebHostService(host))
                        
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
                         .CreateLogger();

                 })
                .UseSerilog()
                .UseDefaultServiceProvider(options => options.ValidateScopes = false) // HACK: Needed for the Razor Email Template rendering
                .UseStartup<Startup>();
        }
    }
}
