using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon.Twitter
{
    /// <summary>
    /// Read the twitter auth token, clientId, ClientSecret from <see cref="IConfiguration"/>
    /// </summary>
    public class TwiterAPIConfiguration
    {

        private const string ConfigKey_TwitterAuthClientId = "TwitterAuthClientId";
        private const string ConfigKey_TwitterAuthClientSecret = "TwitterAuthClientSecret";
        private const string ConfigKey_TwitterAuthToken = "TwitterAuthToken";
        private const string ConfigKey_TwitterAppName = "TwitterAppName";

        private readonly string _authClientId;
        private readonly string _authClientSecret;
        private readonly string _authToken;
        private readonly string _appName;

        private const string DefaultAppName = "TwitterSampleStreamHTTPClient";
        ILogger<TwiterAPIConfiguration> _logger;

        public TwiterAPIConfiguration(IConfiguration configuration, ILogger<TwiterAPIConfiguration> logger)
        {
            _logger = logger;
            _authClientId = configuration.GetValue<string>(ConfigKey_TwitterAuthClientId);

            _authClientId = !string.IsNullOrWhiteSpace(configuration[ConfigKey_TwitterAuthClientId])
                    ? configuration.GetValue<string>(ConfigKey_TwitterAuthClientId)
                    : throw new TypeInitializationException(nameof(TwiterAPIConfiguration), new ArgumentNullException(ConfigKey_TwitterAuthClientId));

            _authClientSecret = !string.IsNullOrWhiteSpace(configuration[ConfigKey_TwitterAuthClientSecret])
                    ? configuration.GetValue<string>(ConfigKey_TwitterAuthClientSecret)
                    : throw new TypeInitializationException(nameof(TwiterAPIConfiguration), new ArgumentNullException(ConfigKey_TwitterAuthClientSecret));

            _authToken = !string.IsNullOrWhiteSpace(configuration[ConfigKey_TwitterAuthToken])
                    ? configuration.GetValue<string>(ConfigKey_TwitterAuthToken)
                    : throw new TypeInitializationException(nameof(TwiterAPIConfiguration), new ArgumentNullException(ConfigKey_TwitterAuthToken));

            _appName = !string.IsNullOrWhiteSpace(configuration[ConfigKey_TwitterAppName])
                ? configuration.GetValue<string>(ConfigKey_TwitterAppName)
                : DefaultAppName;

            _logger.LogDebug($"Twitter API Config APIKey:{_authClientId} ClientSecret:{_authClientSecret} AuthToken:{_authToken} AppName: {_appName}");

        }

        public string AuthClientId => _authClientId;
        public string AuthClientSecret => _authClientSecret;
        public string AuthToken => _authToken;
        public string AppName => _appName;

    }
}
