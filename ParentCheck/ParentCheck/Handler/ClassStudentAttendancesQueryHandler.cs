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
    public class ClassStudentAttendancesQueryHandler : IRequestHandler<ClassStudentAttendancesQuery, ClassStudentAttendancesEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public ClassStudentAttendancesQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<ClassStudentAttendancesEnvelop> Handle(ClassStudentAttendancesQuery studentAttendancesQuery,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            var classStudentAttendances = await classRoomDomain.GetClassStudentAttendancesAsync(studentAttendancesQuery.ClassId, studentAttendancesQuery.RecordDate,studentAttendancesQuery.UserId);

            return new ClassStudentAttendancesEnvelop(classStudentAttendances);
        }
    }
}
