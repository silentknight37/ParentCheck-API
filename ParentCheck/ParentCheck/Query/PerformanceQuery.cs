using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class PerformanceQuery : IRequest<PerformanceEnvelop>
    {
        public PerformanceQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
