using TweetStreamCon.Database;
using TweetStreamCon.Publisher;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace TwitterStreamTest.Publisher
{
    public class ConsolePublisherTest
    {
        private ITestOutputHelper _testOutputLogger;

        public ConsolePublisherTest(ITestOutputHelper testOutputLogger)
        {
            _testOutputLogger = testOutputLogger;
        }

        [Fact]
        public void IRepositoryReader_IsNull_Init_Fail()
        {
            var mockLogger = new Mock<ILogger<ConsolePublisher>>();

            var isAnyException = Record.Exception(() =>
            {
                var publishTwittCount = new ConsolePublisher(null);
            });

            Assert.NotNull(isAnyException);
            Assert.IsType<TypeInitializationException>(isAnyException);
            Assert.IsType<ArgumentNullException>(isAnyException.InnerException);
            Assert.Contains("ILogger<ConsolePublisher>", isAnyException.InnerException.Message);
        }

        [Fact]
        public void IRepositoryReader_RepositoryReader_Return_ValidResult_Success()
        {
            var expectedMessage = "Some Message here";
            var mockLogger = new FakeILogger<ConsolePublisher>(_testOutputLogger);

            var receivedTwitterCount = (new ConsolePublisher(mockLogger)).Publish(expectedMessage, new CancellationToken());
            Assert.Contains(expectedMessage, mockLogger.LastOutPut);
        }

    }
}
