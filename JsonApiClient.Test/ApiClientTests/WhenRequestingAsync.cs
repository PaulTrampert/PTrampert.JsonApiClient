using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace JsonApiClient.Test.ApiClientTests
{
    public class WhenRequestingAsync
    {
        public TestHttpMessageHandler HttpHandler { get; set; }
        public ApiClient Subject { get; set; }

        public WhenRequestingAsync()
        {
            HttpHandler = new TestHttpMessageHandler();

            var httpClient = new HttpClient(HttpHandler);
            Subject = new ApiClient(httpClient);
        }

        [Fact]
        public void ItSendsTheRequest()
        {
            var result = Subject.RequestAsync<object, object>(HttpMethod.Get, "some/uri").Result;
            Assert.Equal(HttpHandler.LastRequest.Method, HttpMethod.Get);
            Assert.Equal(HttpHandler.LastRequest.RequestUri.PathAndQuery, "some/uri");
        }
    }
}
