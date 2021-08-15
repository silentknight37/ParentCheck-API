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
    public class PerformanceQueryHandler : IRequestHandler<PerformanceQuery, PerformanceEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public PerformanceQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<PerformanceEnvelop> Handle(PerformanceQuery performanceQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var performances = await settingDomain.GetPerformance(performanceQuery.UserId);
            return new PerformanceEnvelop(performances);
        }
    }
}
