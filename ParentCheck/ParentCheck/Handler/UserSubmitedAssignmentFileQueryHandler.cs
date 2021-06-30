using MediatR;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Factory;
using ParentCheck.Factory.Intreface;
using ParentCheck.Query;
using System.Threading;
using System.Threading.Tasks;

namespace ParentCheck.Handler
{
    public class UserSubmitedAssignmentFileQueryHandler : IRequestHandler<UserSubmitedAssignmentFileQuery, UserSubmitedAssignmentFileEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public UserSubmitedAssignmentFileQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<UserSubmitedAssignmentFileEnvelop> Handle(UserSubmitedAssignmentFileQuery submitedAssignmentFileQuery,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            var submitedAssignmentFile = await classRoomDomain.GetSubmitedAssignmentFileAsync(submitedAssignmentFileQuery.UserId, submitedAssignmentFileQuery.AssignmentId);

            return new UserSubmitedAssignmentFileEnvelop(submitedAssignmentFile);
        }
    }
}
