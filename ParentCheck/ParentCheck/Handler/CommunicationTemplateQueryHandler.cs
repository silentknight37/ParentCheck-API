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
    public class CommunicationTemplateQueryHandler : IRequestHandler<CommunicationTemplateQuery, CommunicationTemplateEnvelop>
    {
        private readonly ICommunicationFactory communicationFactory;
        private readonly IMediator mediator;

        public CommunicationTemplateQueryHandler(ParentCheckContext parentcheckContext, IMediator mediator)
        {
            this.communicationFactory = new CommunicationFactory(parentcheckContext);
            this.mediator = mediator;
        }

        public async Task<CommunicationTemplateEnvelop> Handle(CommunicationTemplateQuery communicationTemplateQuery, CancellationToken cancellationToken)
        {
            var communicationDomain = this.communicationFactory.Create();
            var communicationInbox = await communicationDomain.GetCommunicationTemplateAsync(communicationTemplateQuery.IsActiveOnly, communicationTemplateQuery.UserId);

            return new CommunicationTemplateEnvelop(communicationInbox);
        }
    }
}
