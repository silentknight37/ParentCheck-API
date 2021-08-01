using MailKit.Net.Smtp;
using MediatR;
using MimeKit;
using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Factory;
using ParentCheck.Factory.Intreface;
using ParentCheck.Query;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ParentCheck.Handler
{
    public class ReplyCommunicationCommandHandler : IRequestHandler<ReplyCommunicationCommand, RequestSaveEnvelop>
    {
        private readonly ICommunicationFactory communicationFactory;
        private readonly IMediator mediator;

        public ReplyCommunicationCommandHandler(ParentCheckContext parentcheckContext, IMediator mediator)
        {
            this.communicationFactory = new CommunicationFactory(parentcheckContext);
            this.mediator = mediator;
        }

        public async Task<RequestSaveEnvelop> Handle(ReplyCommunicationCommand replyCommunicationCommand, CancellationToken cancellationToken)
        {
            var communicationDomain = this.communicationFactory.Create();
            try
            {
                var fromUser = await communicationDomain.GetFromUserCommunicationAsync(replyCommunicationCommand.UserId);

                if (fromUser == null)
                {
                    var errorMessage = "Request fail due to invalid user";
                    Error error = new Error(ErrorType.UNAUTHORIZED, errorMessage);
                    return new RequestSaveEnvelop(false, string.Empty, error);
                }

                var toUser = await communicationDomain.GetFromUserCommunicationAsync(replyCommunicationCommand.ToUserId);

                if (toUser == null)
                {
                    var errorMessage = "Request fail due to invalid user";
                    Error error = new Error(ErrorType.UNAUTHORIZED, errorMessage);
                    return new RequestSaveEnvelop(false, string.Empty, error);
                }

                var response = await communicationDomain.SaveReplyCommunicationAsync(replyCommunicationCommand.GetId, replyCommunicationCommand.Subject, replyCommunicationCommand.MessageText, replyCommunicationCommand.ToUserId, toUser, fromUser, replyCommunicationCommand.UserId);

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
