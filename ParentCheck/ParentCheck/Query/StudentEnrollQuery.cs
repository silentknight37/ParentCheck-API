using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class StudentEnrollQuery : IRequest<StudentEnrollEnvelop>
    {
        public StudentEnrollQuery(long classId,long academicYear,long userId)
        {
            this.ClassId = classId;
            this.AcademicYear = academicYear;
            this.UserId = userId;
        }

        public long ClassId { get; set; }
        public long AcademicYear { get; set; }
        public long UserId { get; set; }
    }
}
