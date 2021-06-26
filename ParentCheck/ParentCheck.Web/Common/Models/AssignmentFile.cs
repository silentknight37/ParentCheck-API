using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class AssignmentFile
    {
        public long Id { get; set; }
        public IFormFile File { get; set; }
    }
}
