using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WebCommon=ParentCheck.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParentCheck.Common;

namespace ParentCheck.Web
{
    public class GlobalExceptionFilter : IExceptionFilter, IDisposable
    {
        private readonly ILogger _logger;

        public GlobalExceptionFilter()
        {
        }

        public void OnException(ExceptionContext context)
        {
            var response = new WebCommon.ApiResponse<string>(
                      null,
                      new List<Error>()
                      {
                        new Error(ErrorType.INTERNAL)
                      }
                    );

            var statusCode = StatusCodes.Status500InternalServerError;

            context.Result = new JsonResult(response)
            {
                StatusCode = statusCode
            };

        }

        public void Dispose()
        {
        }
    }
}
