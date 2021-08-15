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
    public class AcademicClassQueryHandler : IRequestHandler<AcademicClassQuery, AcademicClassEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public AcademicClassQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<AcademicClassEnvelop> Handle(AcademicClassQuery academicClassQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var academicClasses = await settingDomain.GetAcademicClass(academicClassQuery.UserId);
            return new AcademicClassEnvelop(academicClasses);
        }
    }
}
