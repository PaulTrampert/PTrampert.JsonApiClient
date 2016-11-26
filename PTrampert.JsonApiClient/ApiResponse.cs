using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace JsonApiClient
{
    public class ApiResponse<T> : IApiResponse<T>
        where T : class
    {
        public T Data { get; set; }

        public int Status => (int)RawResponse.StatusCode;

        public bool IsSuccessStatus => RawResponse.IsSuccessStatusCode;

        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers => RawResponse.Headers;

        public HttpResponseMessage RawResponse { get; set; }
    }
}
