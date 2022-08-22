using TweetStreamCon.Database;
using Microsoft.Extensions.Logging;

namespace TweetStreamCon.Publisher
{
    /// <summary>
    /// Print/publish the given content to the console.
    /// 
    /// </summary>

    public class ConsolePublisher : IPublisher<string>
    {
        ILogger<ConsolePublisher> _logger;

        public ConsolePublisher(ILogger<ConsolePublisher> logger)
        {
            _logger = logger;
            if (_logger == null)
            {
                throw new TypeInitializationException(nameof(ConsolePublisher), new ArgumentNullException("ILogger<ConsolePublisher>"));
            }
        }

        public async Task Publish(string content, CancellationToken cancellation)
        {
           _logger.LogInformation($"{content}");
        }
    }
}
