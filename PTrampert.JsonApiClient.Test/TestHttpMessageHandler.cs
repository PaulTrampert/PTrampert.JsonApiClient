using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PTrampert.JsonApiClient.Test
{
    public class TestHttpMessageHandler : HttpMessageHandler
    {
        public HttpRequestMessage LastRequest { get; set; }

        public HttpResponseMessage FakeResponse { get; set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;

            return Task.FromResult(FakeResponse);
        }
    }
}
