using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class CalenderEventSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public CalenderEventSaveCommand(DateTime fromDate, DateTime toDate, string subject, string description, int type, long userId)
        {
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.Subject = subject;
            this.Description = description;
            this.Type = type;
            this.UserId = userId;
        }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public long UserId { get; set; }
    }
}
