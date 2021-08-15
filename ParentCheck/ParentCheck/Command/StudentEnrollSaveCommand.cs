using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class StudentEnrollSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public StudentEnrollSaveCommand(long id,long yearAcademic, long classId, long studentId, bool isActive, long userId)
        {
            this.Id = id;
            this.AcademicYear = yearAcademic;
            this.ClassId = classId;
            this.StudentId = studentId;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public long AcademicYear { get; set; }
        public long ClassId { get; set; }
        public long StudentId { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
