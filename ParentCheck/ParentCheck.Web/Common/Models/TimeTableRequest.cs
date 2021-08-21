using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class TimeTableRequest
    {
        public long id { get; set; }
        public long classId { get; set; }
        public long subjectId { get; set; }
        public string fromTime { get; set; }
        public string toTime { get; set; }
        public int weekDayId { get; set; }
    }
}
