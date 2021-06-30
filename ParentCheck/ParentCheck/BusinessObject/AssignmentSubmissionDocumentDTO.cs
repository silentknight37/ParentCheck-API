using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class AssignmentSubmissionDocumentDTO
    {
        public long InstituteAssignmentSubmissionDocumentId { get; set; }
        public long AssignmentSubmissionId { get; set; }        
        public string FileName { get; set; }
        public string EncryptedFileName { get; set; }
        public int? DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public string Url { get; set; }
    }
}
