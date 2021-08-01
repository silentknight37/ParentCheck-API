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
    public class UserCommunicationIndoxQueryHandler : IRequestHandler<UserCommunicationInboxQuery, UserCommunicationEnvelop>
    {
        private readonly ICommunicationFactory communicationFactory;

        public UserCommunicationIndoxQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.communicationFactory = new CommunicationFactory(parentcheckContext);
        }

        public async Task<UserCommunicationEnvelop> Handle(UserCommunicationInboxQuery userCommunicationInboxQuery,CancellationToken cancellationToken)
        {
            var communicationDomain = this.communicationFactory.Create();
            var communicationInbox = await communicationDomain.GetCommunicationInboxAsync(userCommunicationInboxQuery.UserId);

            return new UserCommunicationEnvelop(communicationInbox);
        }
    }
}
