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
    public class CalenderEventQueryHandler : IRequestHandler<CalenderEventQuery, CalenderEventEnvelop>
    {
        private readonly ICalenderFactory calenderFactory;

        public CalenderEventQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.calenderFactory = new CalenderFactory(parentcheckContext);
        }

        public async Task<CalenderEventEnvelop> Handle(CalenderEventQuery calenderEvent,CancellationToken cancellationToken)
        {
            var calenderDomain = this.calenderFactory.Create();
            var calenderEvents = await calenderDomain.GetCalenderEventsAsync(calenderEvent.RequestedDate, calenderEvent.EventType, calenderEvent.UserId);

            return new CalenderEventEnvelop(calenderEvents);
        }
    }
}
