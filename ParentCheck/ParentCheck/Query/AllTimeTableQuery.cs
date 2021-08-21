using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class AllTimeTableQuery : IRequest<TimeTableEnvelop>
    {
        public AllTimeTableQuery(long classId, long userId)
        {
            this.ClassId = classId;
            this.UserId = userId;
        }

        public long ClassId { get; set; }
        public long UserId { get; set; }
    }
}
