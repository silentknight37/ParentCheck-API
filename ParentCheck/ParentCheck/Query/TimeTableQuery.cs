using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class TimeTableQuery : IRequest<TimeTableEnvelop>
    {
        public TimeTableQuery(bool isOnlyToday, long userId)
        {
            this.IsOnlyToday = isOnlyToday;
            this.UserId = userId;
        }

        public bool IsOnlyToday { get; set; }
        public long UserId { get; set; }
    }
}
