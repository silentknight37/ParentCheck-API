using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class ClassStudentAttendancesQuery : IRequest<ClassStudentAttendancesEnvelop>
    {
        public ClassStudentAttendancesQuery(long classId, DateTime recordDate, long userId)
        {
            this.ClassId = classId;
            this.RecordDate = recordDate;
            this.UserId = userId;
        }

        public long ClassId { get; set; }
        public DateTime RecordDate { get; set; }
        public long UserId { get; set; }
    }
}
