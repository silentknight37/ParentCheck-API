using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class AcademicClassDTO
    {
        public long Id { get; set; }
        public string Class { get; set; }
        public int YearAcademic { get; set; }
        public string ResponsibleUser { get; set; }
        public long ResponsibleUserId { get; set; }
        public bool IsActive { get; set; }
    }
}
