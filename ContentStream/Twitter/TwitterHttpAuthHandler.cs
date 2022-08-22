using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon.Twitter
{
    /// <summary>
    /// Attach authorizaion bearer token from <see cref="TwiterAPIConfiguration.AuthToken"/> to the outgoing http calls. 
    /// </summary>

    public class TwitterHttpAuthHandler : DelegatingHandler
    {
        TwiterAPIConfiguration _twiterAPIConfiguration;
        ILogger<TwitterHttpAuthHandler> _logger;
        HttpClient _httpClient;
        public TwitterHttpAuthHandler(HttpClient httpClient, TwiterAPIConfiguration configuration, ILogger<TwitterHttpAuthHandler> logger)
        {
            _twiterAPIConfiguration = configuration;
            _logger = logger;
            _httpClient = httpClient;

        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            
            //base.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _twiterAPIConfiguration.AuthToken);
            request.Headers.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
            request.Headers.Add(HeaderNames.UserAgent, "twtApiTest2003");
            request.Headers.Add(HeaderNames.Authorization, $"Bearer {_twiterAPIConfiguration.AuthToken}");

            try
            {
                var httpResponse = await base.SendAsync(request, cancellationToken);
                if (httpResponse == null)
                {
                    _logger.LogCritical($"Twitter API Call returned null HttpResponse.");
                    return httpResponse;
                }
                if (httpResponse != null && !httpResponse.IsSuccessStatusCode)
                {
                    _logger.LogCritical($"Twitter API Call Failed. HTTP StatusCode: {httpResponse.StatusCode} Response-Content:  {httpResponse.Content}");
                }

                return httpResponse;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Twitter HTTP Call. Exception: {ex}");
                throw;
            }
        }

    }
}
