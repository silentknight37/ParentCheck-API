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
    public class AssignmentFileRemoveCommandHandler : IRequestHandler<AssignmentFileRemoveCommand, RequestSaveEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public AssignmentFileRemoveCommandHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<RequestSaveEnvelop> Handle(AssignmentFileRemoveCommand removeAssignmentFileCommand, CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            try
            {
                var response = await classRoomDomain.RemoveAssignmentFileAsync(removeAssignmentFileCommand.SubmissionId,removeAssignmentFileCommand.Id);

                if (response==0)
                {
                    var errorMessage = "Request fail due to invalid user";
                    Error error = new Error(ErrorType.UNAUTHORIZED, errorMessage);
                    return new RequestSaveEnvelop(false, string.Empty, error);
                }

                return new RequestSaveEnvelop(true, "Request process successfully", response, null);
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
