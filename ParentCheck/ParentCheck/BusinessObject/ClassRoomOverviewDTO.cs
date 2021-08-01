using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class ClassRoomOverviewDTO
    {
        public DateTime Date { get; set; }
        public string Term { get; set; }
        public string Subject { get; set; }
        public string Chapter { get; set; }
        public string Topic { get; set; }
        public long TopicId { get; set; }
    }
}
