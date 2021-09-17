using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class TimeTableDTO
    {
        public long Id { get; set; }
        public string ClassName { get; set; }
        public string Subject { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Weekday { get; set; }
    }
}
