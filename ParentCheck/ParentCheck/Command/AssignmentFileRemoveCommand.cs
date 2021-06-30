using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class AssignmentFileRemoveCommand : IRequest<RequestSaveEnvelop>
    {
        public AssignmentFileRemoveCommand(long submissionId,long assignmentFileId, long userId)
        {
            this.SubmissionId = submissionId;
            this.Id = assignmentFileId;
            this.UserId = userId;
        }

        public long SubmissionId { get; set; }
        public long Id { get; set; }
        public long UserId { get; set; }
    }
}
