using MediatR;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Factory;
using ParentCheck.Factory.Intreface;
using ParentCheck.Query;
using System.Threading;
using System.Threading.Tasks;

namespace ParentCheck.Handler
{
    public class StudentAttendancesQueryHandler : IRequestHandler<StudentAttendancesQuery, ClassStudentAttendancesEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public StudentAttendancesQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<ClassStudentAttendancesEnvelop> Handle(StudentAttendancesQuery studentAttendancesQuery,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            var classStudentAttendances = await classRoomDomain.GetStudentAttendancesAsync(studentAttendancesQuery.UserId);

            return new ClassStudentAttendancesEnvelop(classStudentAttendances);
        }
    }
}
