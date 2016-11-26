using System.Collections.Generic;

namespace PTrampert.JsonApiClient
{
    public interface IApiResponse<T>
        where T : class
    {
        T Data { get; }
        IEnumerable<KeyValuePair<string, IEnumerable<string>>> Headers { get; }
        bool IsSuccessStatus { get; }
        int Status { get; }
    }
}