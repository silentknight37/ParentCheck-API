using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class AssignmentDocumentDTO
    {
        public long Id { get; set; }
        public long InstituteAssignmentId { get; set; }
        public string FileName { get; set; }
        public int AssignmentTypeId { get; set; }
        public string AssignmentType { get; set; }
        public string Url { get; set; }
    }
}
