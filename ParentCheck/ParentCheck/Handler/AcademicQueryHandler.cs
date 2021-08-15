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
    public class AcademicQueryHandler : IRequestHandler<AcademicQuery, AcademicEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public AcademicQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<AcademicEnvelop> Handle(AcademicQuery academicQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var academics = await settingDomain.GetAcademicYear(academicQuery.UserId);
            return new AcademicEnvelop(academics);
        }
    }
}
