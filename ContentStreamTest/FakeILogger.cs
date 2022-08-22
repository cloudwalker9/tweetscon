using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace TwitterStreamTest
{
    public class FakeILogger<T> : ILogger<T>, IDisposable
    {
        private IList<string> _logHistory = new List<string>();
        private ITestOutputHelper _output;
        private string _LastOutput;

        public FakeILogger(ITestOutputHelper output)
        {
             _logHistory = new List<string>();
             _output = output;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _LastOutput = state.ToString();
            _logHistory.Add(state.ToString());
            _output.WriteLine(_LastOutput);
        }

        public IList<string> LogHistory
        {
            get
            {
                return _logHistory;
            }
        }


        public string LastOutPut
        {
            get
            {
                return _LastOutput;
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
