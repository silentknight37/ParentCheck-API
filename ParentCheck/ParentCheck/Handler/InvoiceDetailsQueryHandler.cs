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
    public class InvoiceDetailsQueryHandler : IRequestHandler<InvoiceDetailQuery, InvoiceDetailEnvelop>
    {
        private readonly IPaymentFactory paymentFactory;

        public InvoiceDetailsQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.paymentFactory = new PaymentFactory(parentcheckContext);
        }

        public async Task<InvoiceDetailEnvelop> Handle(InvoiceDetailQuery invoiceDetailQuery,CancellationToken cancellationToken)
        {
            var paymentDomain = this.paymentFactory.Create();
            var invoice = await paymentDomain.GetInvoiceDetailAsync(invoiceDetailQuery.IsGenerated, invoiceDetailQuery.InvoiceId, invoiceDetailQuery.UserId);

            return new InvoiceDetailEnvelop(invoice);
        }
    }
}
