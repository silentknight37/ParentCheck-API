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
    public class UserCommunicationOutboxQueryHandler : IRequestHandler<UserCommunicationOutboxQuery, UserCommunicationEnvelop>
    {
        private readonly ICommunicationFactory communicationFactory;

        public UserCommunicationOutboxQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.communicationFactory = new CommunicationFactory(parentcheckContext);
        }

        public async Task<UserCommunicationEnvelop> Handle(UserCommunicationOutboxQuery userCommunicationOutboxQuery,CancellationToken cancellationToken)
        {
            var communicationDomain = this.communicationFactory.Create();
            var communicationInbox = await communicationDomain.GetCommunicationOutboxAsync(userCommunicationOutboxQuery.UserId);

            return new UserCommunicationEnvelop(communicationInbox);
        }
    }
}
