using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class ReferenceQuery : IRequest<ReferenceEnvelop>
    {
        public ReferenceQuery(long? contextId,int referenceTypeId,long userId)
        {
            this.ContextId = contextId;
            this.ReferenceTypeId = referenceTypeId;
            this.UserId = userId;
        }
        public long? ContextId { get; set; }
        public int ReferenceTypeId { get; set; }
        public long UserId { get; set; }
    }
}
