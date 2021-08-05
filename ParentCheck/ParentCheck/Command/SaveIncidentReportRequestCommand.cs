using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class SaveIncidentReportRequestCommand : IRequest<RequestSaveEnvelop>
    {
        public SaveIncidentReportRequestCommand(long instituteUserId, string subject, string message, long userId)
        {
            this.InstituteUserId = instituteUserId;
            this.Subject = subject;
            this.Message = message;
            this.UserId = userId;
        }

        public long InstituteUserId { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public long UserId { get; set; }
    }
}
