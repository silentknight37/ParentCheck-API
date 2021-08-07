using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class InvoiceTypeQuery : IRequest<InvoiceTypeEnvelop>
    {
        public InvoiceTypeQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
