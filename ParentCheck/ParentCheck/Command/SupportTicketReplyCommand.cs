using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class SupportTicketReplyCommand : IRequest<RequestSaveEnvelop>
    {
        public SupportTicketReplyCommand(string ticketId, string replyMessage, long userId)
        {
            this.TicketId = ticketId;
            this.ReplyMessage = replyMessage;
            this.UserId = userId;
        }

        public string TicketId { get; set; }
        public string ReplyMessage { get; set; }
        public long UserId { get; set; }
        public long GetTicketId
        {
            get
            {
                return string.IsNullOrEmpty(TicketId) ? 0 : long.Parse(TicketId);
            }
        }
    }
}
