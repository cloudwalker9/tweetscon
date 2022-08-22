using TweetStreamCon.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon.Database.SQLLite
{
    /// <summary>
    /// Read SQLLiteDB configuration from <see cref="IConfiguration"/>
    /// Make sure connection string always set to 'Cache=Shared' 
    /// When no connection string found default to <see cref="DEFAULT_CONNECTIONSTRING"/>
    /// </summary>

    public class SQLLiteDBConfiguration : IRepositoryConfiguration
    {
        public const string DEFAULT_DBName = "userTweets.db";
        public const string DEFAULT_CONNECTIONSTRING = $"Data Source=userTweets.db;Cache=Shared";
        public string _connectionString;
        private readonly ILogger<SQLLiteDBConfiguration> _logger;
        private string CONFIGKEY_SqlLiteDBConnectionString = "SqlLiteDBConnectionString";
        public SQLLiteDBConfiguration(IConfiguration configuration, ILogger<SQLLiteDBConfiguration> logger)
        {
            _logger = logger;
            var defaultDbNamePrefix = DateTime.Now.ToString("yyyyMMdd");
            _connectionString = string.IsNullOrWhiteSpace(configuration[CONFIGKEY_SqlLiteDBConnectionString]) 
                ? $"Data Source={defaultDbNamePrefix}{DEFAULT_DBName};Cache=Shared"
                : configuration[CONFIGKEY_SqlLiteDBConnectionString];
            _logger.LogDebug($"SQLLiteDB ConnectionString: {_connectionString}");
        }

        public string ConnectionString { get => _connectionString; set { _connectionString = value; } }
    }
}
