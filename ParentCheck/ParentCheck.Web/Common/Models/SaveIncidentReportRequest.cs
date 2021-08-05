using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class SaveIncidentReportRequest
    {
        public long instituteUserId { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
    }
}
