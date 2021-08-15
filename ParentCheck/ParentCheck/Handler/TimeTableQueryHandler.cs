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
    public class TimeTableQueryHandler : IRequestHandler<TimeTableQuery, TimeTableEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public TimeTableQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<TimeTableEnvelop> Handle(TimeTableQuery timeTableQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var academicClasses = await settingDomain.GetTimeTable(timeTableQuery.IsOnlyToday, timeTableQuery.UserId);
            return new TimeTableEnvelop(academicClasses);
        }
    }
}
