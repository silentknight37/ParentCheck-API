using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class InvoiceQuery : IRequest<InvoiceEnvelop>
    {
        public InvoiceQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
