using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class StudentAttendancesQuery : IRequest<ClassStudentAttendancesEnvelop>
    {
        public StudentAttendancesQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
