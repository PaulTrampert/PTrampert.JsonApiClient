using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;

namespace PTrampert.JsonApiClient.Test.ApiClientTests
{
    [TestFixture]
    public class WhenRequestingAsync
    {
        public TestHttpMessageHandler HttpHandler { get; set; }
        public HttpResponseMessage Response { get; set; }
        public ApiClient Subject { get; set; }

        [SetUp]
        public void Setup()
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

        [Test]
        public async Task ItSendsTheRequest()
        {
            var result = await Subject.RequestAsync<object>(HttpMethod.Get, "some/uri");
            Assert.AreEqual(HttpHandler.LastRequest.Method, HttpMethod.Get);
            Assert.AreEqual(HttpHandler.LastRequest.RequestUri, new Uri("http://localhost/someuri/some/uri"));
        }

        [Test]
        public async Task ItAppendsQueryDataToTheUri()
        {
            var result = await Subject.RequestAsync<object>(HttpMethod.Get, "some/uri", new[] { new KeyValuePair<string, string>("hi", "there"), new KeyValuePair<string, string>("ho", "there") });
            Assert.AreEqual(HttpHandler.LastRequest.RequestUri, new Uri("http://localhost/someuri/some/uri?hi=there&ho=there"));
        }

        [Test]
        public async Task ItDeserializesTheContent()
        {
            var result = await Subject.RequestAsync<dynamic>(HttpMethod.Get, "some/uri");
            Assert.AreEqual(result.Data.something.ToString(), "good");
        }

        [Test]
        public async Task NoContentDoesntCauseException()
        {
            HttpHandler.FakeResponse.Content = null;
            HttpHandler.FakeResponse.StatusCode = System.Net.HttpStatusCode.NoContent;
            var result = await Subject.RequestAsync<dynamic>(HttpMethod.Get, "some/uri");
            Assert.Null(result.Data);
        }

        [Test]
        public async Task NonSuccessStatusCodeThrowsApiException()
        {
            HttpHandler.FakeResponse.StatusCode = System.Net.HttpStatusCode.NotFound;
            var exception = Assert.ThrowsAsync<ApiException>(async () => await Subject.RequestAsync<dynamic>(HttpMethod.Get, "some/uri"));
            Assert.AreSame(HttpHandler.FakeResponse, exception.Response);
        }
    }
}
