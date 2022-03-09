using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VoipServer.Worker
{

    public class DefaultScopedProcessingService : IScopedProcessingService
    {
        private int _executionCount;
        private readonly ILogger<DefaultScopedProcessingService> _logger;

        public DefaultScopedProcessingService(
            ILogger<DefaultScopedProcessingService> logger) =>
            _logger = logger;

        public async System.Threading.Tasks.Task DoWorkAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                ++_executionCount;

                _logger.LogInformation(
                    "{ServiceName} working, execution count: {Count}",
                    nameof(DefaultScopedProcessingService),
                    _executionCount);

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}