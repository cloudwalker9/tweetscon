using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetStreamCon.Database.SQLLite;

namespace TweetStreamCon.Database
{
    /// <summary>
    /// UserTweets Table repository class to insert the user tweets in SQLLITEDB.
    /// 
    /// Table Name: 'userTweets'
    /// Column Schema:
    ///   'Id':  integer - AutoIncrement
    ///   'body': Text 
    ///   'CreatedTimeStamp' :Timestamp - Default to current timestamp.
    /// </summary>

    public class UserTweetsDBRepositoryWriter : IRepositoryWriter<string, string>
    {
        private readonly SQLLiteDBConfiguration _sQlLiteDBConfiguration;
        private readonly ILogger<UserTweetsDBRepositoryWriter> _logger;
        private readonly string  _UserTweetTableName = "userTweets";
        private readonly string _connectionString;

        public UserTweetsDBRepositoryWriter(ILogger<UserTweetsDBRepositoryWriter> logger, SQLLiteDBConfiguration sQlLiteDBConfiguration)
        {

            if (logger == null)
            {
                throw new TypeInitializationException("SQLLightRepositoryWriter", new ArgumentNullException(" ILogger<UserTweetsSQLLiteRepositoryWriter> is null"));
            }
            _sQlLiteDBConfiguration = sQlLiteDBConfiguration;
            _connectionString = new SqliteConnectionStringBuilder(_sQlLiteDBConfiguration.ConnectionString)
                                {
                                    Mode = SqliteOpenMode.ReadWriteCreate,
                                }.ToString();

            _logger = logger;
            var isTableCreated = CreateTableIfNotExists().Result;
            if (!isTableCreated)
                throw new TypeInitializationException("SQLLightRepositoryWriter", new ApplicationException($"Table '{_UserTweetTableName}' failed."));
        }

        public  Task<bool> CreateTableIfNotExists()
        {
            using (var connection = new SqliteConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var command = connection.CreateCommand();
                    command.CommandText =
                    @$"
                    CREATE TABLE IF NOT EXISTS {_UserTweetTableName} (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        body TEXT NULL,
                        CreatedTimeStamp DATETIME DEFAULT current_timestamp
                    );
                ";
                    command.ExecuteNonQueryAsync();
                    return Task.FromResult(true);
                }
                catch (Exception ex)
                {
                    _logger?.LogError($"Exception during {_UserTweetTableName} table creation. Exception. {ex}");
                    return Task.FromResult(false);
                }
            }
        }

        private  SqliteCommand PrepareInsertCommand(SqliteCommand command)
        {
            command.CommandText =
            @$"
                    INSERT INTO {_UserTweetTableName}(body) 
                    VALUES ($body)
           ";
                
            var parameter1 = command.CreateParameter();
            parameter1.ParameterName = "$body";
            command.Parameters.Add(parameter1);

            return command;
        }

        /// <summary>
        ///  Insert value into Table:<see cref="_UserTweetTableName"/>
        /// </summary>

        public async Task<string> Insert(string body)
        {
            if (string.IsNullOrWhiteSpace(body))
            {
                _logger.LogWarning($"{nameof(UserTweetsDBRepositoryWriter)}.Insert() received null argument. Operation skipped");
                return "failed";
            }
            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = PrepareInsertCommand(connection.CreateCommand()))
                {
                    try
                    {
                        cmd.Parameters["$body"].Value = body; //Todo: Parse the body with datatransformation logic
                        await cmd.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Table '{_UserTweetTableName}' Insert failed. Exception {ex}");
                    }
                    //await transaction.CommitAsync();
                }
            }
  
            return "success";
        }
    }
}
 