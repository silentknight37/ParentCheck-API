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
    public class InvoiceTypeQueryHandler : IRequestHandler<InvoiceTypeQuery, InvoiceTypeEnvelop>
    {
        private readonly IPaymentFactory paymentFactory;

        public InvoiceTypeQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.paymentFactory = new PaymentFactory(parentcheckContext);
        }

        public async Task<InvoiceTypeEnvelop> Handle(InvoiceTypeQuery invoiceTypeQuery,CancellationToken cancellationToken)
        {
            var paymentDomain = this.paymentFactory.Create();
            var invoiceTypes = await paymentDomain.GetInvoiceTypesAsync(invoiceTypeQuery.UserId);

            return new InvoiceTypeEnvelop(invoiceTypes);
        }
    }
}
