using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class CompleteAssignmentCommand : IRequest<RequestSaveEnvelop>
    {
        public CompleteAssignmentCommand(long assignmentId, long userId)
        {
            this.AssignmentId = assignmentId;
            this.UserId = userId;
        }

        public long AssignmentId { get; set; }
        public long UserId { get; set; }
    }
}
