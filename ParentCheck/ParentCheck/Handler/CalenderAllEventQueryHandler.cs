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
    public class CalenderAllEventQueryHandler : IRequestHandler<CalenderAllEventQuery, CalenderEventEnvelop>
    {
        private readonly ICalenderFactory calenderFactory;

        public CalenderAllEventQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.calenderFactory = new CalenderFactory(parentcheckContext);
        }

        public async Task<CalenderEventEnvelop> Handle(CalenderAllEventQuery calenderAllEventQuery,CancellationToken cancellationToken)
        {
            var calenderDomain = this.calenderFactory.Create();
            var calenderEvents = await calenderDomain.GetCalenderAllEventsAsync(calenderAllEventQuery.UserId);

            return new CalenderEventEnvelop(calenderEvents);
        }
    }
}
