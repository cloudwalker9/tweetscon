using TweetStreamCon.Database;
using TweetStreamCon.Publisher;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon.Twitter
{
    /// <summary>
    /// Read the totall twitter count from  <see cref="IUserTweetsRepositoryReader"/> implementation and publish it via <see cref="IPublisher<string>"/>
    /// </summary>

    public class TwitterCountPublisher : IProcessor
    {
        private readonly ILogger<TwitterCountPublisher> _logger;
        private readonly IUserTweetsRepositoryReader _userTweetsRepositoryReader;
        IPublisher<string> _publisher;

        public TwitterCountPublisher(ILogger<TwitterCountPublisher> logger, IPublisher<string> publisher, IUserTweetsRepositoryReader userTweetsRepositoryReader)
        {
            
            _userTweetsRepositoryReader = userTweetsRepositoryReader;
            if (_userTweetsRepositoryReader == null)
            {
                throw new TypeInitializationException(nameof(TwitterCountPublisher), new ArgumentNullException("IUserTweetsRepositoryReader"));
            }
            _logger = logger;

            _publisher = publisher;
            if (_publisher == null)
            {
                throw new TypeInitializationException(nameof(TwitterCountPublisher), new ArgumentNullException("IPublisher<string>"));
            }

        }
        public async Task Process(CancellationToken cancellation)
        {
            _logger.LogDebug($"{nameof(TwitterCountPublisher)} Execution Begin");
            //Get the Total Count
            var TotalCount = await _userTweetsRepositoryReader.GetTotalCount(cancellation);
            //Get processed count per minute
            var CountPerMinute = await _userTweetsRepositoryReader.GetTotalCountByInterval(cancellation, 1);
            await _publisher.Publish($"**************{DateTime.Now:yyyy-MM-ddTHH:mm:ss} Tweet Count: {TotalCount} ************** Processed / Minute:  {CountPerMinute}", cancellation);
            _logger.LogDebug($"{nameof(TwitterCountPublisher)} Execution End");
        }



    }
}
