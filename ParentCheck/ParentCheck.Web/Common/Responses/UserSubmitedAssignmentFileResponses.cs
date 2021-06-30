using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class UserSubmitedAssignmentFileResponses
    {
        public long id { get; set; }
        public DateTime? completeDate { get; set; }
        public bool assignmentCompleted { get; set; }
        public string status { get; set; }
        public List<SubmissionDocument> submissionDocuments { get; set; }

        public static UserSubmitedAssignmentFileResponses PopulateSubmitedAssignmentFileResponses(UserSubmitedAssignmentFileDTO submitedAssignmentFile)
        {
            var submitedAssignmentFileResponses = new UserSubmitedAssignmentFileResponses();
            submitedAssignmentFileResponses.id = submitedAssignmentFile.AssignmentSubmissionId;
            submitedAssignmentFileResponses.completeDate = submitedAssignmentFile.CompleteDate;
            submitedAssignmentFileResponses.assignmentCompleted = submitedAssignmentFile.CompleteDate.HasValue;
            submitedAssignmentFileResponses.status = submitedAssignmentFile.StatusText;
            submitedAssignmentFileResponses.submissionDocuments = new List<SubmissionDocument>();

            foreach (var document in submitedAssignmentFile.AssignmentSubmissionDocuments)
            {
                var content = new SubmissionDocument
                {
                    id = document.InstituteAssignmentSubmissionDocumentId,
                    typeId = document.DocumentTypeId,
                    type = document.DocumentType,
                    fileName = document.FileName,
                    enFileName = document.EncryptedFileName,
                    url = document.Url,
                    submissionId= document.AssignmentSubmissionId
                };

                submitedAssignmentFileResponses.submissionDocuments.Add(content);
            }

            return submitedAssignmentFileResponses;
        }
    }

    public class SubmissionDocument
    {
        public long id { get; set; }
        public long submissionId { get; set; }
        public string fileName { get; set; }
        public string enFileName { get; set; }
        public int? typeId { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }
}
