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
    public class StudentEnrollHandler : IRequestHandler<StudentEnrollQuery, StudentEnrollEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public StudentEnrollHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<StudentEnrollEnvelop> Handle(StudentEnrollQuery studentEnrollQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var academics = await settingDomain.GetStudentEnroll(studentEnrollQuery.ClassId, studentEnrollQuery.AcademicYear, studentEnrollQuery.UserId);
            return new StudentEnrollEnvelop(academics);
        }
    }
}
