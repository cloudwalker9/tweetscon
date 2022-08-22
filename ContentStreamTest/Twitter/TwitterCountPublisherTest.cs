using TweetStreamCon.Database;
using TweetStreamCon.Publisher;
using TweetStreamCon.Twitter;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;
using TweetStreamCon.Twitter;

namespace TwitterStreamTest.Publisher
{
    public class TwitterCountPublisherTest
    {
        private ITestOutputHelper _testOutputLogger;

        public TwitterCountPublisherTest(ITestOutputHelper testOutputLogger)
        {
            _testOutputLogger = testOutputLogger;
        }

        [Fact]
        public void IRepositoryReader_IsNull_Init_Fail()
        {
            var mockLogger = new Mock<ILogger<TwitterCountPublisher>>();
            var mockpublisherLogger = new Mock<ILogger<ConsolePublisher>>();

            IPublisher<string> publisher = new ConsolePublisher(mockpublisherLogger.Object);//, IUserTweetsRepositoryReader userTweetsRepositoryReader

            var isAnyException = Record.Exception(() =>
            {
                var publishTwittCount = new TwitterCountPublisher(mockLogger.Object, publisher, null);
            });

            Assert.NotNull(isAnyException);
            Assert.IsType<TypeInitializationException>(isAnyException);
            Assert.IsType<ArgumentNullException>(isAnyException.InnerException);
            Assert.Contains("IUserTweetsRepositoryReader", isAnyException.InnerException.Message);
        }

        [Fact]
        public void IRepositoryReader_RepositoryReader_Return_ValidResult_Success()
        {
            var expectedCount = 20;
            var mockLogger = new Mock<ILogger<TwitterCountPublisher>>();
            var mockpublisherLogger = new FakeILogger<ConsolePublisher>(_testOutputLogger); //new Mock<ILogger<ConsolePublisher>>();
            IPublisher<string> publisher = new ConsolePublisher(mockpublisherLogger);//, IUserTweetsRepositoryReader userTweetsRepositoryReader
            var  TweetRepositoryReader = new Mock<IUserTweetsRepositoryReader>();
            _ = TweetRepositoryReader.Setup(w => w.GetTotalCount(It.IsAny<CancellationToken>()))
                .Returns(
                    (CancellationToken token) =>
                    {
                        return Task.FromResult(expectedCount);
                    }
                );

            var publishTwittCount = new TwitterCountPublisher(mockLogger.Object, publisher, TweetRepositoryReader.Object);
            var token = new CancellationToken();
            publishTwittCount.Process(token).GetAwaiter().GetResult();
            Assert.True(mockpublisherLogger.LogHistory.Where(str => str.Contains($"{expectedCount.ToString()}")).Any());
        }

    }
}
