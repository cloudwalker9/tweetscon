using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TwitterStreamTest.Twitter
{
    public class FakeHttpMessageHandler : DelegatingHandler
    {
        protected HttpResponseMessage _mockResponse = null;
        protected List<HttpRequestMessage> _receivedRequests = new List<HttpRequestMessage>();
        private Exception _exceptionToThrow = null;
        public FakeHttpMessageHandler(HttpStatusCode code, string message)
        {
            HttpResponseMessage resp = new HttpResponseMessage(code);
            resp.Content = new StringContent(message);
            _mockResponse = resp;
        }

        public FakeHttpMessageHandler(HttpResponseMessage responseMessage)
        {
            _mockResponse = responseMessage;
        }

        public FakeHttpMessageHandler(Exception exception)
        {
            _exceptionToThrow = exception;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //add the request in th collection.
            if (request != null)
                _receivedRequests.Add(request);

            //Throw Exception 
            if (_exceptionToThrow != null)
                throw _exceptionToThrow;

            //Return response
            if (_mockResponse != null)
                return await Task.FromResult(_mockResponse);

            return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
        }

        public int TotalRequestReceived => _receivedRequests.Count;

    }

}
