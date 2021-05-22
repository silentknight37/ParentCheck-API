using System.Text.Json.Serialization;

namespace ParentCheck.Common
{
    public class ApiResponse<T>
    {
        public bool hasErrors;
        public T Body;
        [JsonIgnore]
        public string RawRequest;
        [JsonIgnore]
        public string RawResponse;

        public ApiResponse(T body, bool hasErrors)
        {
            this.Body = body;
            this.hasErrors = hasErrors;
        }
    }
}
