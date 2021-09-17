using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class StudentEnrollDTO
    {
        public long Id { get; set; }
        public string StudentUserName { get; set; }
        public long StudentUserId { get; set; }
        public string IndexNo { get; set; }
        public string ClassName { get; set; }
        public long ClassId { get; set; }
        public long AcademicYearId { get; set; }
        public bool IsActive { get; set; }
    }
}
