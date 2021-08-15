using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class StudentEnrollRequest
    {
        public long id { get; set; }
        public long academicYear { get; set; }
        public long classId { get; set; }
        public long studentId { get; set; }
        public bool isActive { get; set; }
    }
}
