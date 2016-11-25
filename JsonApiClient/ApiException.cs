using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace JsonApiClient
{
    public class ApiException : Exception
    {
        public HttpResponseMessage Response { get; }

        public ApiException(HttpResponseMessage response) : base($"The API's response status did not indicate success: {response.StatusCode}")
        {
            Response = response;
        }
    }
}
