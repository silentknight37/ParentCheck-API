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
    public class SaveIncidentReportRequestCommandHandler : IRequestHandler<SaveIncidentReportRequestCommand, RequestSaveEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public SaveIncidentReportRequestCommandHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<RequestSaveEnvelop> Handle(SaveIncidentReportRequestCommand saveIncidentReportRequestCommand,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            try
            {
                var response = await classRoomDomain.SaveIncidentReportAsync(saveIncidentReportRequestCommand.InstituteUserId, saveIncidentReportRequestCommand.Subject, saveIncidentReportRequestCommand.Message, saveIncidentReportRequestCommand.UserId);
               
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
                return new RequestSaveEnvelop(false,string.Empty, error);
            }
        }
    }
}
