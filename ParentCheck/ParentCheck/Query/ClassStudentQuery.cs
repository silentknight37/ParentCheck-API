using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class ClassStudentQuery : IRequest<ClassStudentEnvelop>
    {
        public ClassStudentQuery(long classId, long userId)
        {
            this.ClassId = classId;
            this.UserId = userId;
        }

        public long ClassId { get; set; }
        public long UserId { get; set; }
    }
}
