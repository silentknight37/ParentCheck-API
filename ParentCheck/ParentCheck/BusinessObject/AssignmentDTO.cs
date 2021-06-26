using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class AssignmentDTO
    {
        public long Id { get; set; }
        public string AssignmentTitle { get; set; }
        public string AssignmentDescription { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public List<AssignmentDocumentDTO> AssignmentDocuments { get; set; }
    }
}
