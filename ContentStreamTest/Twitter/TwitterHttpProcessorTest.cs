using TweetStreamCon.Twitter;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TwitterStreamTest;
using TweetStreamCon.Database;
using Moq;
using TweetStreamCon;
using Xunit.Abstractions;
using System.Net.Sockets;

namespace TwitterStreamTest.Twitter
{
    public class TwitterHttpProcessorTest
    {
        private ITestOutputHelper _testOutputLogger;

        public TwitterHttpProcessorTest(ITestOutputHelper testOutputLogger)
        {
            _testOutputLogger = testOutputLogger;
        }

        [Fact]
        public void HttpResponse_Unsuccessfull_Fail()
        {
            //public TwitterHttpProcessor(HttpClient httpClient, IRepositoryWriter<string, string> UserTweetsRepositoryWriter, HttpStreamReaderConfig config, ILogger<TwitterHttpProcessor> logger)

            var mockProcessorLogger = new FakeILogger<TwitterHttpProcessor>(_testOutputLogger); //new Mock<ILogger<ConsolePublisher>>();
            var repositoryWriter = new Mock<IRepositoryWriter<string, string>>();
            var httpResponseExpected = new HttpResponseMessage(HttpStatusCode.Forbidden);
            var fakeHttpClientHandler = new FakeHttpMessageHandler(httpResponseExpected);
            var config = new HttpStreamReaderConfig()
            {
                URL = "https://someuri"
            };
            var cheetahHttpClient = new HttpClient(fakeHttpClientHandler);
            IProcessor processor = new TwitterHttpProcessor(cheetahHttpClient, repositoryWriter.Object, config, mockProcessorLogger);
            processor.Process(new CancellationToken()).GetAwaiter().GetResult();
            Assert.True(mockProcessorLogger.LogHistory.Where(str => str.Contains($"{HttpStatusCode.Forbidden}")).Any());
        }


        [Fact]
        public void HttpCall_Throw_Socket_Exception()
        {
            var expectedError = "Http Called failed. Exception returned";
            var mockProcessorLogger = new FakeILogger<TwitterHttpProcessor>(_testOutputLogger); //new Mock<ILogger<ConsolePublisher>>();
            var repositoryWriter = new Mock<IRepositoryWriter<string, string>>();
            var fakeHttpClientHandler = new FakeHttpMessageHandler(new Exception(expectedError));
            var config = new HttpStreamReaderConfig()
            {
                URL = "https://someuri"
            };
            var cheetahHttpClient = new HttpClient(fakeHttpClientHandler);
            IProcessor processor = new TwitterHttpProcessor(cheetahHttpClient, repositoryWriter.Object, config, mockProcessorLogger);
            processor.Process(new CancellationToken()).GetAwaiter().GetResult();
            Assert.True(mockProcessorLogger.LogHistory.Where(str => str.Contains(expectedError)).Any());
        }

    }
}
