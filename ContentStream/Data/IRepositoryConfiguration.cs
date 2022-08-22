using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TweetStreamCon.Database
{
    /// <summary>
    /// Repository interface to hold connection string value
    /// </summary>

    interface IRepositoryConfiguration
    {
        public string ConnectionString { get; }

    }

}
