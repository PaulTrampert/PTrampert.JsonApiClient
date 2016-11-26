using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PTrampert.JsonApiClient
{
    public interface IApiClient
    {
        Task<IApiResponse<TResponse>> DeleteAsync<TResponse>(string uri, object data = null, IEnumerable<KeyValuePair<string, string>> queryData = null) where TResponse : class;
        Task<IApiResponse<TResponse>> GetAsync<TResponse>(string uri, IEnumerable<KeyValuePair<string, string>> queryData = null) where TResponse : class;
        Task<IApiResponse<TResponse>> PatchAsync<TResponse>(string uri, object data = null, IEnumerable<KeyValuePair<string, string>> queryData = null) where TResponse : class;
        Task<IApiResponse<TResponse>> PostAsync<TResponse>(string uri, object data = null, IEnumerable<KeyValuePair<string, string>> queryData = null) where TResponse : class;
        Task<IApiResponse<TResponse>> PutAsync<TResponse>(string uri, object data = null, IEnumerable<KeyValuePair<string, string>> queryData = null) where TResponse : class;
        Task<IApiResponse<TResponse>> RequestAsync<TResponse>(HttpMethod httpMethod, string uri, IEnumerable<KeyValuePair<string, string>> queryData = null, object data = null) where TResponse : class;
    }
}