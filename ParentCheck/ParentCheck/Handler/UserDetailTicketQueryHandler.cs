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
    public class UserDetailTicketQueryHandler : IRequestHandler<UserDetailTicketQuery, DetailTicketEnvelop>
    {
        private readonly ISupportTicketFactory supportTicketFactory;

        public UserDetailTicketQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.supportTicketFactory = new SupportTicketFactory(parentcheckContext);
        }

        public async Task<DetailTicketEnvelop> Handle(UserDetailTicketQuery userDetailTicketQuery,CancellationToken cancellationToken)
        {
            var supportTicketDomain = this.supportTicketFactory.Create();
            var supportTickets = await supportTicketDomain.GetDetailTicketsAsync(userDetailTicketQuery.Id, userDetailTicketQuery.UserId);

            return new DetailTicketEnvelop(supportTickets);
        }
    }
}
