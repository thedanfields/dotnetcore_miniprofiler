using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using System;
using profiler.common;

namespace profiler.service
{
    class Startup
    {
        private readonly ILogger<Startup> _logger;
        private IConfiguration Configuration { get; }

        public Startup(ILogger<Startup> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddMemoryCache();

            var appConfigurationSection = Configuration.GetSection("Application");
            services.Configure<ApplicationSettings>(appConfigurationSection);

            var p = services.BuildServiceProvider();

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profile";
                options.Storage = new LoggingMemoryCacheStorage(
                    p.GetRequiredService<ILogger<LoggingMemoryCacheStorage>>(),
                    p.GetRequiredService<IMemoryCache>(),
                    TimeSpan.FromMinutes(60));
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiniProfiler();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
    