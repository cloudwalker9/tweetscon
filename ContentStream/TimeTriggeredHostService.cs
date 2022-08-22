using TweetStreamCon.Publisher;
using TweetStreamCon.Twitter;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Timer triggered host service. The trigger interval value can be changed in  "appsetting.json" with keyname TimerHostServiceInterval 
/// </summary>

namespace TweetStreamCon
{
    public class TimeTriggeredHostService : IHostedService, IDisposable
    {
        private int executionCount = 0;
        private readonly ILogger<TimeTriggeredHostService> _logger;
        private Timer? _timer = null;
        private IProcessor _userTweetReadProcessor;
        private readonly int _timerIntervalInSeconds;
        private const int TIMER_INTERVAL_DEFAULT_INSECONDS = 10;
        public string ConfigKey_TimerHostServiceInterval = "TimerHostServiceInterval";
        public TimeTriggeredHostService(ILogger<TimeTriggeredHostService> logger, TwitterCountPublisher userTweetReadProcessor, IConfiguration configuration)
        {
            _userTweetReadProcessor = userTweetReadProcessor;
            _logger = logger;

            int TimerIntervalInSeconds = configuration.GetValue<int>(ConfigKey_TimerHostServiceInterval);

            _timerIntervalInSeconds = TimerIntervalInSeconds <= 0 ? TIMER_INTERVAL_DEFAULT_INSECONDS : TimerIntervalInSeconds;
            if (_userTweetReadProcessor == null)
            {
                throw new TypeInitializationException(nameof(TimeTriggeredHostService), new ArgumentNullException("IProcessor UserTweetReadProcessor"));
            }
            if (_logger == null)
            {
                throw new TypeInitializationException(nameof(TimeTriggeredHostService), new ArgumentNullException("ILogger<TimerHostService>"));
            }

            _logger.LogInformation($"Initilalized HostService: {nameof(TimeTriggeredHostService)}. Interval Seconds {_timerIntervalInSeconds}");
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Starting host service: {nameof(TimeTriggeredHostService)}");
            _timer = new Timer(TimedProcessCaller, stoppingToken, TimeSpan.Zero, TimeSpan.FromSeconds(_timerIntervalInSeconds));
            return Task.CompletedTask;
        }

        private async void TimedProcessCaller(object? cancellationtoken)
        {
            var count = Interlocked.Increment(ref executionCount);
            var token = (CancellationToken)cancellationtoken;
            await _userTweetReadProcessor.Process(token);
            _logger.LogDebug($"Executed HostService: {nameof(TimeTriggeredHostService)}.  Count: {count}");
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Stopping host service: {nameof(TimeTriggeredHostService)}");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
