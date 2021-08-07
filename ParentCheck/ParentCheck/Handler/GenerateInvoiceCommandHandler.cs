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
    public class GenerateInvoiceCommandHandler : IRequestHandler<GenerateInvoiceCommand, RequestSaveEnvelop>
    {
        private readonly IPaymentFactory paymentFactory;
        private readonly ICommunicationFactory communicationFactory;
        private readonly IMediator mediator;

        public GenerateInvoiceCommandHandler(ParentCheckContext parentcheckContext, IMediator mediator)
        {
            this.paymentFactory = new PaymentFactory(parentcheckContext);
            this.communicationFactory = new CommunicationFactory(parentcheckContext);
            this.mediator = mediator;
        }

        public async Task<RequestSaveEnvelop> Handle(GenerateInvoiceCommand generateInvoiceCommand, CancellationToken cancellationToken)
        {
            var paymentDomain = this.paymentFactory.Create();
            var communicationDomain = this.communicationFactory.Create();
            try
            {
                var toUser = generateInvoiceCommand.ToUsers;
                if (generateInvoiceCommand.IsGroup)
                {
                    toUser = await communicationDomain.GetToUserCommunicationAsync(generateInvoiceCommand.ToGroups, generateInvoiceCommand.UserId);
                }
                var response = await paymentDomain.GenerateInvoiceAsync(
                    generateInvoiceCommand.InvoiceTitle,
                    generateInvoiceCommand.InvoiceDetails,
                    toUser,
                    generateInvoiceCommand.DueDate,
                    generateInvoiceCommand.InvoiceDate,
                    generateInvoiceCommand.InvoiceAmount,
                    generateInvoiceCommand.InvoiceTypeId,
                    generateInvoiceCommand.UserId
                    );

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
