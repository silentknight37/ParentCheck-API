using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class AcademicClassRequest
    {
        public long id { get; set; }
        public string academicClass { get; set; }
        public long yearAcademic { get; set; }
        public long responsibleUserId { get; set; }
        public bool isActive { get; set; }
    }
}
