using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class CalenderEventDTO
    {
        public long Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public string ColorCode { get; set; }
    }
}
