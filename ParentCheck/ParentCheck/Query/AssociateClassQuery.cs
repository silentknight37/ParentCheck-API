using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class AssociateClassQuery : IRequest<AssociateClassEnvelop>
    {
        public AssociateClassQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
