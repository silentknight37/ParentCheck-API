using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class TimeTableSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public TimeTableSaveCommand(long id,long classId, long subjectId, string fromTime, string toTime, int weekDayId, long userId)
        {
            this.Id = id;
            this.ClassId = classId;
            this.SubjectId = subjectId;
            this.FromTime = fromTime;
            this.ToTime = toTime;
            this.WeekDayId = weekDayId;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public long ClassId { get; set; }
        public long SubjectId { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public int WeekDayId { get; set; }
        public long UserId { get; set; }
    }
}
