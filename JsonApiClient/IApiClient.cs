using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace JsonApiClient
{
    public interface IApiClient
    {
        Task<IApiResponse<TResponse>> DeleteAsync<TResponse, TRequest>(string uri, TRequest data, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
            where TRequest : class;
        Task<IApiResponse<TResponse>> GetAsync<TResponse>(string uri, IEnumerable<KeyValuePair<string, string>> queryData = null) where TResponse : class;
        Task<IApiResponse<TResponse>> RequestAsync<TResponse, TRequest>(HttpMethod httpMethod, string uri, TRequest data = null, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
            where TRequest : class;
        Task<IApiResponse<TResponse>> PatchAsync<TResponse, TRequest>(string uri, TRequest data, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
            where TRequest : class;
        Task<IApiResponse<TResponse>> PostAsync<TResponse, TRequest>(string uri, TRequest data, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
            where TRequest : class;
        Task<IApiResponse<TResponse>> PutAsync<TResponse, TRequest>(string uri, TRequest data, IEnumerable<KeyValuePair<string, string>> queryData = null)
            where TResponse : class
            where TRequest : class;
    }
}