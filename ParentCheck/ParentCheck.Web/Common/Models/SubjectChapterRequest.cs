using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class SubjectChapterRequest
    {
        public long id { get; set; }
        public long subjectId { get; set; }
        public string chapter { get; set; }
        public bool isActive { get; set; }
    }
}
