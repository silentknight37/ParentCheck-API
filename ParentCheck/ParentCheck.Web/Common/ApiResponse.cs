using ParentCheck.Common;
using System.Collections.Generic;
using System.Linq;

namespace ParentCheck.Web.Common
{
    public class ApiResponse<T> : ParentCheck.Common.ApiResponse<T>
    {
        public IEnumerable<Error> Errors;

        public ApiResponse(T body, bool hasErrors) : base(body, hasErrors)
        {
            this.Errors = new List<Error>();
            this.hasErrors = hasErrors;
            this.Body = body;
        }

        /// <summary>
        /// No error response / Create response
        /// </summary>
        public ApiResponse(T body) : base(body, false)
        {
            this.hasErrors = false;
            this.Body = body;
            this.Errors = new List<Error>();
        }

        public ApiResponse(T body, List<Error> errors) : base(body, errors?.Any() ?? false)
        {
            this.hasErrors = errors?.Any() ?? false;
            this.Body = body;
            Errors = errors.AsEnumerable();
        }
    }
}
