using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class InvoiceDetailQuery : IRequest<InvoiceDetailEnvelop>
    {
        public InvoiceDetailQuery(bool isGenerated, long invoiceId,long userId)
        {
            this.IsGenerated = isGenerated;
            this.InvoiceId = invoiceId;
            this.UserId = userId;
        }

        public bool IsGenerated { get; set; }
        public long InvoiceId { get; set; }
        public long UserId { get; set; }
    }
}
