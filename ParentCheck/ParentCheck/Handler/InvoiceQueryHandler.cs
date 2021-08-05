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
    public class InvoiceQueryHandler : IRequestHandler<InvoiceQuery, InvoiceEnvelop>
    {
        private readonly IPaymentFactory paymentFactory;

        public InvoiceQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.paymentFactory = new PaymentFactory(parentcheckContext);
        }

        public async Task<InvoiceEnvelop> Handle(InvoiceQuery invoiceQuery,CancellationToken cancellationToken)
        {
            var paymentDomain = this.paymentFactory.Create();
            var invoices = await paymentDomain.GetInvoiceAsync(invoiceQuery.UserId);

            return new InvoiceEnvelop(invoices);
        }
    }
}
