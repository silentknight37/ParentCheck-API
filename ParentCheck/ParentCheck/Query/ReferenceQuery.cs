using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class ReferenceQuery : IRequest<ReferenceEnvelop>
    {
        public ReferenceQuery(int referenceTypeId,long userId)
        {
            this.ReferenceTypeId = referenceTypeId;
            this.UserId = userId;
        }

        public int ReferenceTypeId { get; set; }
        public long UserId { get; set; }
    }
}
