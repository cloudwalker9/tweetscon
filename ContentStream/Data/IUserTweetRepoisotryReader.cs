using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon.Database
{
    /// <summary>
    /// Repository interface to get TotalRecordCount and Total Record count between time duration.
    /// </summary>

    public interface IUserTweetsRepositoryReader
    {
        public Task<int> GetTotalCount(CancellationToken cancellationToken);
        public Task<int> GetTotalCountByInterval(CancellationToken cancellationToken, int intervalInMinutes);
    }

}
