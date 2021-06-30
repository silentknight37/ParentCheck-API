using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class UserSubmitedAssignmentFileDTO
    {
        public UserSubmitedAssignmentFileDTO()
        {
            AssignmentSubmissionDocuments = new List<AssignmentSubmissionDocumentDTO>();
        }
        public long AssignmentSubmissionId { get; set; }
        public DateTime? CompleteDate { get; set; }
        public int? StatusId { get; set; }
        public string StatusText { get; set; }
        public List<AssignmentSubmissionDocumentDTO> AssignmentSubmissionDocuments { get; set; }
    }
}
