using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Destructurama;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace profiler.web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                var env = hostingContext.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
            })
            .UseStartup<Startup>()

            .UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration
                           .ReadFrom.Configuration(hostingContext.Configuration)
                           //.ReadFrom.ConfigurationSection(hostingContext.Configuration.GetSection("Metrics"))

                           .Enrich.FromLogContext()
                           // needed for miniprofiler logging
                           .Destructure.JsonNetTypes();

                loggerConfiguration
                           .Enrich.FromLogContext()
                           .ReadFrom.ConfigurationSection(hostingContext.Configuration.GetSection("Metrics"))

                           // needed for miniprofiler logging
                           .Destructure.JsonNetTypes();

               
            });
    }
}
