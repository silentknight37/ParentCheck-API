using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using ParentCheck.Common.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common
{
    public static class AppHelper
    {        static AppHelper()
        {
            _app = new App<AppSettings>();
        }

        private static App<AppSettings> _app;

        public static AppSettings Settings => _app.Settings;

        public static IConfiguration Configuration => _app.Configuration;

        // Application Details
        public const string ApplicationId = "ParentCheckAPI";

        // Date Types
        public const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";

        // Content Types
        public const string ContentTypePDF = "application/pdf";

        public const string ContentTypeXLS = "application/vnd.ms-excel";

        public static DateTime FromJSString(this string value)
        {
            return DateTime.ParseExact(value, AppHelper.DateTimeFormat, CultureInfo.InvariantCulture);
        }

        public static DateTime ExecutionDateTime(this HttpRequest request)
        {
            DateTime executionDateTime = request.Headers["requestdate"][0].FromJSString();
            return executionDateTime;
        }
    }
}
