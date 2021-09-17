using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class FilterClassRoom
    {
        
        public bool isToday { get; set; }
        public bool isThisWeek { get; set; }
        public bool isNextWeek { get; set; }
        public bool isCustom { get; set; }
        public string? fromDate { get; set; }
        public string? toDate { get; set; }
        public string subjectId { get; set; }
        public string instituteTermsId { get; set; }
    }
}
