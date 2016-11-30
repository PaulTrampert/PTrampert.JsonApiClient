using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace PTrampert.JsonApiClient
{
    public class ApiClient : IApiClient
    {
        private HttpClient httpClient;

        private JsonSerializerSettings jsonSettings;

        public ApiClient(HttpClient httpClient, JsonSerializerSettings jsonSettings = null)
        {
            this.httpClient = httpClient;
            this.jsonSettings = jsonSettings;
        }

        public async Task<IApiResponse<TResponse>> RequestAsync<TResponse>(
            HttpMethod httpMethod,
            string uri,
            IEnumerable<KeyValuePair<string, string>> queryData = null,
            object data = null)
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
            if (!response.IsSuccessStatusCode)
            {
                throw new ApiException(response); 
            }
            string content = null;
            if (response.Content != null)
            {
                content = await response.Content.ReadAsStringAsync();
            }

            return new ApiResponse<TResponse>
            {
                RawResponse = response,
                Data = string.IsNullOrWhiteSpace(content) ? null : JsonConvert.DeserializeObject<TResponse>(content)
            };
        }

        public Task<IApiResponse<TResponse>> GetAsync<TResponse>(string uri, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
        {
            return RequestAsync<TResponse>(HttpMethod.Get, uri, queryData: queryData);
        }

        public Task<IApiResponse<TResponse>> PostAsync<TResponse>(string uri, object data = null, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
        {
            return RequestAsync<TResponse>(HttpMethod.Post, uri, data: data, queryData: queryData);
        }

        public Task<IApiResponse<TResponse>> PutAsync<TResponse>(string uri, object data = null, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
        {
            return RequestAsync<TResponse>(HttpMethod.Put, uri, data: data, queryData: queryData);
        }

        public Task<IApiResponse<TResponse>> PatchAsync<TResponse>(string uri, object data = null, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
        {
            return RequestAsync<TResponse>(new HttpMethod("PATCH"), uri, data: data, queryData: queryData);
        }

        public Task<IApiResponse<TResponse>> DeleteAsync<TResponse>(string uri, object data = null, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
        {
            return RequestAsync<TResponse>(HttpMethod.Delete, uri, data: data, queryData: queryData);
        }
    }
}
