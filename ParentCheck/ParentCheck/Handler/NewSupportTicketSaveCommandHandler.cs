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
    public class NewSupportTicketSaveCommandHandler : IRequestHandler<NewSupportTicketSaveCommand, RequestSaveEnvelop>
    {
        private readonly ISupportTicketFactory supportTicketFactory;

        public NewSupportTicketSaveCommandHandler(ParentCheckContext parentcheckContext)
        {
            this.supportTicketFactory = new SupportTicketFactory(parentcheckContext);
        }

        public async Task<RequestSaveEnvelop> Handle(NewSupportTicketSaveCommand supportTicketSaveCommand,CancellationToken cancellationToken)
        {
            var supportTicketDomain = this.supportTicketFactory.Create();
            try
            {
                var response = await supportTicketDomain.NewSupportTicketAsync(supportTicketSaveCommand.Subject, supportTicketSaveCommand.IssueText, supportTicketSaveCommand.UserId);
               
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
