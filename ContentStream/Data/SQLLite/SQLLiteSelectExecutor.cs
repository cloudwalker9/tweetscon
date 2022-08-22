using TweetStreamCon.Database;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon.Database.SQLLite
{

    /// <summary>
    /// Implements <see cref="IRepositoryReader<string>"/> to perform read operation on SQLLiteDB
    /// </summary>
    public class SQLLiteSelectStatementExecutor : IRepositoryReader<string>
    {
        private readonly SQLLiteDBConfiguration _sQlLiteDBConfiguration;
        private readonly ILogger<SQLLiteSelectStatementExecutor> _logger;
        private const string TABLENAME_USERTWEET = "userTweets";
        private const string QUERY_MAXID = "maxiId";
        private const string QUERY_TotalCount = "TotalCount";

        private readonly string _connectionString;

        public SQLLiteSelectStatementExecutor(ILogger<SQLLiteSelectStatementExecutor> logger, SQLLiteDBConfiguration sQlLiteDBConfiguration)
        {

            if (logger == null)
            {
                throw new TypeInitializationException(nameof(SQLLiteSelectStatementExecutor), new ArgumentNullException("ILogger<UserTweetsSQLLiteRepositoryReader> is null"));
            }

            if (sQlLiteDBConfiguration == null)
            {
                throw new TypeInitializationException(nameof(SQLLiteSelectStatementExecutor), new ArgumentNullException("SQLLiteDBConfiguration is null"));
            }
            _sQlLiteDBConfiguration = sQlLiteDBConfiguration;
            _connectionString = new SqliteConnectionStringBuilder(_sQlLiteDBConfiguration.ConnectionString)
            {
                Mode = SqliteOpenMode.ReadOnly
            }.ToString();
            _logger = logger;
        }

        public SqliteCommand PrepareSelectCommand(string inputSQL, SqliteCommand command)
        {
            command.CommandText = inputSQL;
            return command;
        }

        public async Task Get(string inputSQL, CancellationToken cancellationToken, Func<DbDataReader, Task> readFromDbDataReader)
        {
            if (readFromDbDataReader == null)
                return;

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var cmd = PrepareSelectCommand(inputSQL, connection.CreateCommand()))
                {
                    try
                    {
                        var DBreader = await cmd.ExecuteReaderAsync(cancellationToken);
                        while (DBreader.Read())
                        {
                            await readFromDbDataReader(DBreader);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Table '{TABLENAME_USERTWEET}' find total failed. Exception {ex}");
                    }
                }
            }
        }
    }
}
