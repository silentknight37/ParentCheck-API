using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class InvoiceQuery : IRequest<InvoiceEnvelop>
    {
        public InvoiceQuery(bool isGenerated,long userId)
        {
            this.IsGenerated = isGenerated;
            this.UserId = userId;
        }

        public bool IsGenerated { get; set; }
        public long UserId { get; set; }
    }
}
