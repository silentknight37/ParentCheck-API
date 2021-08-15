using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class CalenderTodayEventQuery : IRequest<CalenderEventEnvelop>
    {
        public CalenderTodayEventQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
