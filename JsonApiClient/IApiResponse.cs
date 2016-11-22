using System.Collections.Generic;

namespace CiTasks.Apis
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