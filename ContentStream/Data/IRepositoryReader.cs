using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon.Database
{
    /// <summary>
    /// Repository interface to perform read operation.
    /// </summary>
    /// <param name="readDataFromDataReader"> Function pointer to process the data from <see cref="DbDataReader"/> </param>
    public interface IRepositoryReader<T>
    {
        public Task  Get(T input, CancellationToken cancellationToken, Func<DbDataReader, Task> readDataFromDataReader);
    }
}
