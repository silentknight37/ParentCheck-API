using MediatR;
using ParentCheck.Common;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Factory;
using ParentCheck.Factory.Intreface;
using ParentCheck.Query;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ParentCheck.Handler
{
    public class CompleteAssignmentCommandHandler : IRequestHandler<CompleteAssignmentCommand, RequestSaveEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public CompleteAssignmentCommandHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<RequestSaveEnvelop> Handle(CompleteAssignmentCommand completeAssignmentCommand, CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            try
            {
                var response = await classRoomDomain.CompleteAssignment(completeAssignmentCommand.AssignmentId, completeAssignmentCommand.UserId);

                if (!response)
                {
                    var errorMessage = "Request fail due to invalid user";
                    Error error = new Error(ErrorType.UNAUTHORIZED, errorMessage);
                    return new RequestSaveEnvelop(false, string.Empty, error);
                }

                return new RequestSaveEnvelop(response, "Request process successfully", null);
            }
            catch (System.Exception e)
            {
                var errorMessage = e.Message;
                Error error = new Error(ErrorType.BAD_REQUEST, errorMessage);
                return new RequestSaveEnvelop(false, string.Empty, error);
            }
        }
    }
}
