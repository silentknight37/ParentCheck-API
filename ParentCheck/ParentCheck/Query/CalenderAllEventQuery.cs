using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class CalenderAllEventQuery : IRequest<CalenderEventEnvelop>
    {
        public CalenderAllEventQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
