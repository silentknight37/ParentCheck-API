using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class SubjectQuery : IRequest<SubjectEnvelop>
    {
        public SubjectQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
