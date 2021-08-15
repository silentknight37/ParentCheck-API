using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class AcademicClassSubjectDTO
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public long SubjectId { get; set; }
        public string ResponsibleUser { get; set; }
        public long ResponsibleUserId { get; set; }
        public bool IsActive { get; set; }
    }
}
