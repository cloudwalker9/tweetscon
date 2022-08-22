using TweetStreamCon;
using TweetStreamCon.Twitter;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon
{
    /// <summary>
    /// Host service that consume Twitter http stream. 
    /// </summary>
    public sealed class TwittConsumerHostService : IHostedService, IAsyncDisposable
    {
        private readonly Task _completedTask = Task.CompletedTask;
        private readonly ILogger<TwittConsumerHostService> _logger;
        private int _executionCount = 0;
        private readonly IProcessor _processor;

        public TwittConsumerHostService(ILogger<TwittConsumerHostService> logger, TwitterHttpProcessor processor)
        { 
            _logger = logger;
            _processor = processor;

        }

        public async  Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("{Service} is running.", nameof(TwittConsumerHostService));
             await _processor.Process(stoppingToken);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Stopped HostService {nameof(TwittConsumerHostService)}");
            return _completedTask;
        }

        public async ValueTask DisposeAsync()
        {
            _logger.LogInformation($"Disposed HostService {nameof(TwittConsumerHostService)} .");
            await _completedTask;
        }
    }
}
