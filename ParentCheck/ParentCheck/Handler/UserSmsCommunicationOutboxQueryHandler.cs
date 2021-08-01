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
    public class UserSmsCommunicationOutboxQueryHandler : IRequestHandler<UserSmsCommunicationOutboxQuery, UserCommunicationEnvelop>
    {
        private readonly ICommunicationFactory communicationFactory;

        public UserSmsCommunicationOutboxQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.communicationFactory = new CommunicationFactory(parentcheckContext);
        }

        public async Task<UserCommunicationEnvelop> Handle(UserSmsCommunicationOutboxQuery userSmsCommunicationOutboxQuery,CancellationToken cancellationToken)
        {
            var communicationDomain = this.communicationFactory.Create();
            var communicationInbox = await communicationDomain.GetSMSCommunicationOutboxAsync(userSmsCommunicationOutboxQuery.UserId);

            return new UserCommunicationEnvelop(communicationInbox);
        }
    }
}
