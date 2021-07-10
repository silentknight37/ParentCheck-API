using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class NewSupportTicketSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public NewSupportTicketSaveCommand( string subject, string issueText, long userId)
        {
            this.Subject = subject;
            this.IssueText = issueText;
            this.UserId = userId;
        }

        public string Subject { get; set; }
        public string IssueText { get; set; }
        public long UserId { get; set; }
    }
}
