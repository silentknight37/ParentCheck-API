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
    public class CalenderTodayEventQueryHandler : IRequestHandler<CalenderTodayEventQuery, CalenderEventEnvelop>
    {
        private readonly ICalenderFactory calenderFactory;

        public CalenderTodayEventQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.calenderFactory = new CalenderFactory(parentcheckContext);
        }

        public async Task<CalenderEventEnvelop> Handle(CalenderTodayEventQuery calenderEvent,CancellationToken cancellationToken)
        {
            var calenderDomain = this.calenderFactory.Create();
            var calenderEvents = await calenderDomain.GetCalenderTodayEventsAsync(calenderEvent.UserId);

            return new CalenderEventEnvelop(calenderEvents);
        }
    }
}
