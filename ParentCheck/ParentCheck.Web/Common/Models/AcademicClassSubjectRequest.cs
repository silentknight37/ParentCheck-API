using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class AcademicClassSubjectRequest
    {
        public long id { get; set; }
        public long classId { get; set; }
        public long subjectId { get; set; }
        public long responsibleUserId { get; set; }
        public bool isActive { get; set; }
    }
}
