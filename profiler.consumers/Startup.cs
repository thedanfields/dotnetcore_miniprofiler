using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using profiler.consumers.Models.Settings;
using profiler.consumers.Services;
using System;

namespace profiler.consumers
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddOptions();

            services.AddHostedService<BusService>();
            ConfigureConsumers(services);
        }

        private void ConfigureConsumers(IServiceCollection services)
        {
            services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var busSettings = provider.GetRequiredService<IOptions<BusSettings>>().Value;

                var host = cfg.Host(new Uri(busSettings.BuildEndpoint()), h =>
                {
                    h.Username(busSettings.UserName);
                    h.Password(busSettings.Password);
                });
            }));

            services.AddSingleton<IBus>(provider => provider.GetRequiredService<IBusControl>());
            services.AddSingleton(provider => new Lazy<IBus>(provider.GetService<IBus>));
        }
    }
}
