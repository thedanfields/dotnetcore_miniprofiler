using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace profiler.service
{
    public class ApplicationSettings
    {
        public string Name { get; set; }
    }

    public class WorkflowAppWebHostService : WebHostService
    {
        private ILogger _logger;
        private IOptions<ApplicationSettings> _settings;

        public WorkflowAppWebHostService(IWebHost host) : base(host)
        {
            _logger = host.Services.GetRequiredService<ILogger<WorkflowAppWebHostService>>();
            _settings = host.Services.GetRequiredService<IOptions<ApplicationSettings>>();
        }

        protected override void OnStarting(string[] args)
        {
            _logger.LogInformation("{ApplicationName} Starting", _settings.Value.Name);

            base.OnStarting(args);
        }

        protected override void OnStarted()
        {
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            _logger.LogInformation("{ApplicationName} Stopping", _settings.Value.Name);
            base.OnStopping();
        }
    }
}
