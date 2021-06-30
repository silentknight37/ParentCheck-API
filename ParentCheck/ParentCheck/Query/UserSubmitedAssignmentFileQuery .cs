using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserSubmitedAssignmentFileQuery : IRequest<UserSubmitedAssignmentFileEnvelop>
    {
        public UserSubmitedAssignmentFileQuery(long userId, long assignmentId)
        {
            this.UserId = userId;
            this.AssignmentId = assignmentId;
        }

        public long UserId { get; set; }
        public long AssignmentId { get; set; }
    }
}
