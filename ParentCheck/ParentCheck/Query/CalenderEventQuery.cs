using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class CalenderEventQuery : IRequest<CalenderEventEnvelop>
    {
        public CalenderEventQuery(DateTime requestedDate,int eventType,long userId)
        {
            this.RequestedDate = requestedDate;
            this.EventType = eventType;
            this.UserId = userId;
        }

        public DateTime RequestedDate { get; set; }
        public int EventType { get; set; }
        public long UserId { get; set; }
    }
}
