using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class UserSubjectDTO
    {
        public long InstituteClassSubjectId { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string ColorBg { get; set; }
        public string ColorFont { get; set; }
    }
}
