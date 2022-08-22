using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon.Database
{

    /// <summary>
    ///  Perform read operation from Table:<see cref="TABLENAME_USERTWEET"/>
    /// </summary>
    
    public class UserTweetsRepositoryReader : IUserTweetsRepositoryReader
    {
        private readonly ILogger<UserTweetsRepositoryReader> _logger;
        private const string TABLENAME_USERTWEET = "userTweets";
        private const string QUERY_MAXID = "maxiId";
        private const string QUERY_TotalCount = "TotalCount";
        IRepositoryReader<string> _repositoryReader;

        public UserTweetsRepositoryReader(ILogger<UserTweetsRepositoryReader> logger, IRepositoryReader<string> repositoryReader)
        {
            _logger = logger;
            if (_logger == null)
            {
                throw new TypeInitializationException(nameof(UserTweetsRepositoryReader), new ArgumentNullException("ILogger<UserTweetsSQLLiteRepositoryReader> is null"));
            }
            _repositoryReader = repositoryReader;
            if (_repositoryReader == null)
            {
                throw new TypeInitializationException(nameof(UserTweetsRepositoryReader), new ArgumentNullException("IRepositoryReader<string, int>  is null"));
            }
        }

        /// <summary>
        /// Get Total Record count from table '<see cref="TABLENAME_USERTWEET"/>'
        /// </summary>
        public async Task<int> GetTotalCount(CancellationToken cancellationToken)
        {
            var cancelToken = new CancellationToken();
            var TotalCount = 0;

            var inputSQL = @$"SELECT MAX(id) as {QUERY_MAXID},  count(id) as {QUERY_TotalCount}, STRFTIME('%s', Max(CreatedTimeStamp)) as maxTimeStamp,  STRFTIME('%s','2022-08-21T18:48:22') as convertedTime  FROM {TABLENAME_USERTWEET}";

            async Task ReadTotalCountFromDataReader(DbDataReader dataReader)
            {
                if (dataReader == null)
                    await Task.FromResult(false);
                TotalCount = dataReader.GetFieldValue<int>(dataReader.GetOrdinal(QUERY_TotalCount));
                await Task.FromResult(true);
            }

            await   _repositoryReader.Get(inputSQL, cancelToken, ReadTotalCountFromDataReader);
            return TotalCount;
        }

        /// <summary>
        /// Get Total Record count by given interval from table '<see cref="TABLENAME_USERTWEET"/>'
        /// Perform filters by 'CreatedTimeStamp'
        /// </summary>
        public async Task<int> GetTotalCountByInterval(CancellationToken cancellationToken, int intervalPerMinutes)
        {
            var sqlLiteDateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
            var cancelToken = new CancellationToken();
            var TotalCount = 0;
            var dateTimeNow = DateTime.Now;
            var FromDate = dateTimeNow.AddMinutes(-1 * intervalPerMinutes).ToString(sqlLiteDateTimeFormat);
            var ToDate = dateTimeNow.ToString(sqlLiteDateTimeFormat);

            _logger.LogDebug($"====>DateTime Filter between '{FromDate}' and '{ToDate}'<=====");

            var inputSQL = @$"SELECT count(id) as {QUERY_TotalCount}  FROM {TABLENAME_USERTWEET} WHERE  STRFTIME('%s',CreatedTimeStamp) > STRFTIME('%s','{FromDate}') AND  STRFTIME('%s',CreatedTimeStamp) > STRFTIME('%s','{ToDate}')";//
            //AND datetime('{ToDate}')";
            //strftime('%s', CreatedTimeStamp) BETWEEN strftime('%s', '{FromDate}') AND strftime('%s', '{ToDate}')"; //BETWEEN '{FromDate}' AND '{ToDate}' ORDERBY  CreatedTimeStamp";

            async Task ReadTotalCountFromDataReader(DbDataReader dataReader)
            {
                if (dataReader == null)
                    await Task.FromResult(false);
                TotalCount = dataReader.GetFieldValue<int>(dataReader.GetOrdinal(QUERY_TotalCount));
                await Task.FromResult(true);
            }
            await _repositoryReader.Get(inputSQL, cancelToken, ReadTotalCountFromDataReader);

            return TotalCount;
        }

    }
}
 