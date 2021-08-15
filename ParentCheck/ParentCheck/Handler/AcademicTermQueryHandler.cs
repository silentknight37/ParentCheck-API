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
    public class AcademicTermQueryHandler : IRequestHandler<AcademicTermQuery, AcademicTermEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public AcademicTermQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<AcademicTermEnvelop> Handle(AcademicTermQuery academicTermQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var academicTerms = await settingDomain.GetAcademicTerm(academicTermQuery.UserId);
            return new AcademicTermEnvelop(academicTerms);
        }
    }
}
