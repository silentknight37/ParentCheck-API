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
    public class IncidentReportQueryHandler : IRequestHandler<IncidentReportQuery, IncidentReportEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public IncidentReportQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<IncidentReportEnvelop> Handle(IncidentReportQuery incidentReportQuery,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            var incidentReports = await classRoomDomain.GetIncidentReports(incidentReportQuery.UserId);

            return new IncidentReportEnvelop(incidentReports);
        }
    }
}
