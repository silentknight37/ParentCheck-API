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
    public class UserSupportTicketQueryHandler : IRequestHandler<UserSupportTicketQuery, SupportTicketEnvelop>
    {
        private readonly ISupportTicketFactory supportTicketFactory;

        public UserSupportTicketQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.supportTicketFactory = new SupportTicketFactory(parentcheckContext);
        }

        public async Task<SupportTicketEnvelop> Handle(UserSupportTicketQuery userSupportTicketQuery,CancellationToken cancellationToken)
        {
            var supportTicketDomain = this.supportTicketFactory.Create();
            var supportTickets = await supportTicketDomain.GetTicketsAsync(userSupportTicketQuery.TicketType, userSupportTicketQuery.UserId);

            return new SupportTicketEnvelop(supportTickets);
        }
    }
}
