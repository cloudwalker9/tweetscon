using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon.Database
{
    /// <summary>
    /// Repository interface to persist given value.
    /// </summary>
    public interface IRepositoryWriter<T, R>
    {
        public Task<R> Insert(T input);

    }
}
