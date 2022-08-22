using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon.Publisher
{
    /// <summary>
    /// Publisher contract
    /// </summary>
    public interface IPublisher<T>
    {
        public Task Publish(T content, CancellationToken cancellation);
    }
}
