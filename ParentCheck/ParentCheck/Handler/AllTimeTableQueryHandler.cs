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
    public class AllTimeTableQueryHandler : IRequestHandler<AllTimeTableQuery, TimeTableEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public AllTimeTableQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<TimeTableEnvelop> Handle(AllTimeTableQuery timeTableQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var timeTables = await settingDomain.GetAllTimeTable(timeTableQuery.ClassId, timeTableQuery.UserId);
            return new TimeTableEnvelop(timeTables);
        }
    }
}
