using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class AcademicTermDTO
    {
        public long Id { get; set; }
        public string Term { get; set; }
        public int YearAcademic { get; set; }
        public long YearAcademicId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public bool IsActive { get; set; }
    }
}
