using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class AcademicTermRequest
    {
        public long id { get; set; }
        public string term { get; set; }
        public long yearAcademic { get; set; }
        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public bool isActive { get; set; }
    }
}
