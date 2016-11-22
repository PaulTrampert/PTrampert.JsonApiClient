using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Reflection;

namespace CiTasks.Apis
{
    public class ApiClient : IApiClient
    {
        private HttpClient httpClient;

        private JsonSerializerSettings jsonSettings;

        public ApiClient(HttpClient httpClient = null, JsonSerializerSettings jsonSettings = null)
        {
            this.httpClient = httpClient;
            this.jsonSettings = jsonSettings;
        }

        public async Task<IApiResponse<TResponse>> RequestAsync<TResponse, TRequest>(
            HttpMethod httpMethod,
            string uri,
            TRequest data = null,
            IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TRequest : class
            where TResponse : class
        {
            var finalUri = uri;
            if (queryData != null)
            {
                finalUri += $"?{new FormUrlEncodedContent(queryData)}";
            }
            var request = new HttpRequestMessage(httpMethod, finalUri);
            if (data != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data, Formatting.None, jsonSettings), Encoding.UTF8, "application/json");
            }
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();

            return new ApiResponse<TResponse>
            {
                RawResponse = response,
                Data = string.IsNullOrWhiteSpace(content) ? null : JsonConvert.DeserializeObject<TResponse>(content)
            };
        }

        public Task<IApiResponse<TResponse>> GetAsync<TResponse>(string uri, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
        {
            return RequestAsync<TResponse, object>(HttpMethod.Get, uri, queryData: queryData);
        }

        public Task<IApiResponse<TResponse>> PostAsync<TResponse, TRequest>(string uri, TRequest data, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
            where TRequest : class
        {
            return RequestAsync<TResponse, object>(HttpMethod.Post, uri, data: data, queryData: queryData);
        }

        public Task<IApiResponse<TResponse>> PutAsync<TResponse, TRequest>(string uri, TRequest data, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
            where TRequest : class
        {
            return RequestAsync<TResponse, object>(HttpMethod.Put, uri, data: data, queryData: queryData);
        }

        public Task<IApiResponse<TResponse>> PatchAsync<TResponse, TRequest>(string uri, TRequest data, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
            where TRequest : class
        {
            return RequestAsync<TResponse, object>(new HttpMethod("PATCH"), uri, data: data, queryData: queryData);
        }

        public Task<IApiResponse<TResponse>> DeleteAsync<TResponse, TRequest>(string uri, TRequest data, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
            where TRequest : class
        {
            return RequestAsync<TResponse, object>(HttpMethod.Delete, uri, data: data, queryData: queryData);
        }
    }
}
