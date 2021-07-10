using MediatR;
using ParentCheck.Common;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserSupportTicketQuery : IRequest<SupportTicketEnvelop>
    {
        public UserSupportTicketQuery(EnumSupportTicketType ticketType, long userId)
        {
            this.TicketType = ticketType;
            this.UserId = userId;
        }

        public EnumSupportTicketType TicketType { get; set; }
        public long UserId { get; set; }
    }
}
