using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class FilterClassRoom
    {
        public DateTime? fromDate { get; set; }
        public DateTime? toDate { get; set; }
        public string subjectId { get; set; }
        public string instituteTermsId { get; set; }
    }
}
