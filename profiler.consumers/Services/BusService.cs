using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace profiler.consumers.Services
{
    public class BusService : IHostedService
    {
        private readonly IBusControl _busControl;
        private readonly ILogger _logger;
        private readonly IApplicationLifetime _appLifetime;

        public BusService(ILogger<BusService> logger,

            IApplicationLifetime appLifetime,
            IBusControl busControl)
        {
            _logger = logger;
            _busControl = busControl;
            _appLifetime = appLifetime;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var handle = await _busControl.StartAsync(cancellationToken);
                await handle.Ready;

                _logger.LogInformation("Bus at {busAddress} started", _busControl.Address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to connect to bus at {busAddress}.  Stopping application", _busControl.Address);
                _appLifetime.StopApplication();
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _busControl.StopAsync(cancellationToken);
            _logger.LogInformation("Bus at {busAddress} stopped", _busControl.Address);
        }
    }
}
