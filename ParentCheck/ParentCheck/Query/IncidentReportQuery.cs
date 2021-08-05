using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class IncidentReportQuery : IRequest<IncidentReportEnvelop>
    {
        public IncidentReportQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
