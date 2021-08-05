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
    public class ClassStudentQueryHandler : IRequestHandler<ClassStudentQuery, ClassStudentEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public ClassStudentQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<ClassStudentEnvelop> Handle(ClassStudentQuery classStudentQuery,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            var classStudents = await classRoomDomain.GetClassStudentAsync(classStudentQuery.ClassId, classStudentQuery.UserId);

            return new ClassStudentEnvelop(classStudents);
        }
    }
}
