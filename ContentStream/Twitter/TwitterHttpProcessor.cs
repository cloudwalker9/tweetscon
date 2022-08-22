using TweetStreamCon.Database;
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
    /// Used as configuration data holder by <see cref="TwitterHttpProcessor"/>
    /// </summary>
    public class HttpStreamReaderConfig
    {
        public string URL;
    }

    /// <summary>
    /// Reading http stream response from <see cref="HttpStreamReaderConfig.URL"/> and pass it as input to <see cref="UserTweetsRepositoryWriter"/>
    /// </summary>
    public class TwitterHttpProcessor : IProcessor
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TwitterHttpProcessor> _logger;
        private readonly HttpStreamReaderConfig _httpStreamReaderConfig;
        private IRepositoryWriter<string, string> _userTweetsRepositoryWriter;
        public TwitterHttpProcessor(HttpClient httpClient, IRepositoryWriter<string, string> UserTweetsRepositoryWriter, HttpStreamReaderConfig config, ILogger<TwitterHttpProcessor> logger)
        {
            _userTweetsRepositoryWriter = UserTweetsRepositoryWriter;
            _httpClient = httpClient;
            _httpStreamReaderConfig = config;
            _logger = logger;
        }

        public async Task Process(CancellationToken cancellation)
        {
            _logger.LogInformation($"{nameof(TwitterHttpProcessor)} Execution Begin");
            var httpGetRequest = new HttpRequestMessage(HttpMethod.Get, _httpStreamReaderConfig.URL);
            try
            {
                using (var response = await _httpClient.SendAsync(httpGetRequest, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError($"**** {nameof(TwitterHttpProcessor)} Http Response: {_httpStreamReaderConfig.URL} : StatusCode: {response?.StatusCode}. Retry logic not implemented yet. You may need to restart the service :D.*****");
                        return;
                    }

                    using (var body = await response.Content.ReadAsStreamAsync())
                    {
                        using (var reader = new StreamReader(body))
                        {
                            while (!cancellation.IsCancellationRequested && !reader.EndOfStream)
                            {
                                var input = reader.ReadLine();
                                if (!string.IsNullOrEmpty(input))
                                {
                                    _ = await _userTweetsRepositoryWriter.Insert(input);
                                }
                            }
                        }
                    }
                     
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"*** Http Call: {_httpStreamReaderConfig.URL} Failed. Exception {ex}. Restart the service. ****");
            }
            _logger.LogInformation($"{nameof(TwitterHttpProcessor)} Execution End");
        }



    }
}
