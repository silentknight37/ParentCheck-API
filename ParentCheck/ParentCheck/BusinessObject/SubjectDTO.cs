using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class SubjectDTO
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public string DescriptionText { get; set; }
        public bool IsActive { get; set; }
    }
}
