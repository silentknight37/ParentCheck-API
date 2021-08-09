using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class AcademicSubjectRequest
    {
        public long id { get; set; }
        public string subject { get; set; }
        public string descriptionText { get; set; }
        public bool isActive { get; set; }
    }
}
