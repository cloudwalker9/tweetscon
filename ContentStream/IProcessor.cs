using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon
{
    /// <summary>
    /// Processor host service interface.
    /// </summary>
    public interface IProcessor
    {
        public Task Process(CancellationToken cancellation);
    }
}
