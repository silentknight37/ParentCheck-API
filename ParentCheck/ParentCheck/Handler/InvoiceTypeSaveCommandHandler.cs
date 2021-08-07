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
    public class InvoiceTypeSaveCommandHandler : IRequestHandler<InvoiceTypeSaveCommand, RequestSaveEnvelop>
    {
        private readonly IPaymentFactory paymentFactory;
        private readonly IMediator mediator;

        public InvoiceTypeSaveCommandHandler(ParentCheckContext parentcheckContext, IMediator mediator)
        {
            this.paymentFactory = new PaymentFactory(parentcheckContext);
            this.mediator = mediator;
        }

        public async Task<RequestSaveEnvelop> Handle(InvoiceTypeSaveCommand invoiceTypeSaveCommand, CancellationToken cancellationToken)
        {
            var paymentDomain = this.paymentFactory.Create();
            try
            {
                var response = await paymentDomain.InvoiceTypeSaveAsync(
                    invoiceTypeSaveCommand.Id,
                    invoiceTypeSaveCommand.TypeText,
                    invoiceTypeSaveCommand.Terms,
                    invoiceTypeSaveCommand.isActive,
                    invoiceTypeSaveCommand.UserId
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
