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
        public HttpResponseMessage Response { get; set; }
        public ApiClient Subject { get; set; }

        public WhenRequestingAsync()
        {
            HttpHandler = new TestHttpMessageHandler();

            HttpHandler.FakeResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            HttpHandler.FakeResponse.Content = new StringContent("{ 'something': 'good' }");

            var httpClient = new HttpClient(HttpHandler)
            {
                BaseAddress = new Uri("http://localhost/someuri/")
            };
            Subject = new ApiClient(httpClient);
        }

        [Fact]
        public async Task ItSendsTheRequest()
        {
            var result = await Subject.RequestAsync<object>(HttpMethod.Get, "some/uri");
            Assert.Equal(HttpHandler.LastRequest.Method, HttpMethod.Get);
            Assert.Equal(HttpHandler.LastRequest.RequestUri, new Uri("http://localhost/someuri/some/uri"));
        }

        [Fact]
        public async Task ItDeserializesTheContent()
        {
            var result = await Subject.RequestAsync<dynamic>(HttpMethod.Get, "some/uri");
            Assert.Equal(result.Data.something.ToString(), "good");
        }

        [Fact]
        public async Task NoContentDoesntCauseException()
        {
            HttpHandler.FakeResponse.Content = null;
            HttpHandler.FakeResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
            var result = await Subject.RequestAsync<dynamic>(HttpMethod.Get, "some/uri");
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task NonSuccessStatusCodeThrowsApiException()
        {
            HttpHandler.FakeResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            var exception = await Assert.ThrowsAsync<ApiException>(async () => await Subject.RequestAsync<dynamic>(HttpMethod.Get, "some/uri"));
            Assert.Same(HttpHandler.FakeResponse, exception.Response);
        }
    }
}
