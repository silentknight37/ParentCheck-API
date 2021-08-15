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
    public class ClassSubjectQueryHandler : IRequestHandler<ClassSubjectQuery, AcademicClassSubjectEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public ClassSubjectQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<AcademicClassSubjectEnvelop> Handle(ClassSubjectQuery classSubjectQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var academicClassSubjects = await settingDomain.GetClassSubject(classSubjectQuery.ClassId,classSubjectQuery.UserId);
            return new AcademicClassSubjectEnvelop(academicClassSubjects);
        }
    }
}
