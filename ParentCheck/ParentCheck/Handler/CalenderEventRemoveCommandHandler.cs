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
    public class CalenderEventRemoveCommandHandler : IRequestHandler<CalenderEventRemoveCommand, RequestSaveEnvelop>
    {
        private readonly ICalenderFactory calenderFactory;

        public CalenderEventRemoveCommandHandler(ParentCheckContext parentcheckContext)
        {
            this.calenderFactory = new CalenderFactory(parentcheckContext);
        }

        public async Task<RequestSaveEnvelop> Handle(CalenderEventRemoveCommand calenderEventRemoveCommand, CancellationToken cancellationToken)
        {
            var calenderDomain = this.calenderFactory.Create();
            try
            {
                var response = await calenderDomain.RemoveCalenderEventAsync(calenderEventRemoveCommand.Id, calenderEventRemoveCommand.UserId);

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
