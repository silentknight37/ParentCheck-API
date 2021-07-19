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
    public class UserCommunicationDetailQueryHandler : IRequestHandler<UserCommunicationDetailQuery, UserCommunicationDetailEnvelop>
    {
        private readonly ICommunicationFactory communicationFactory;

        public UserCommunicationDetailQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.communicationFactory = new CommunicationFactory(parentcheckContext);
        }

        public async Task<UserCommunicationDetailEnvelop> Handle(UserCommunicationDetailQuery userCommunicationDetailQuery,CancellationToken cancellationToken)
        {
            var communicationDomain = this.communicationFactory.Create();
            var communicationInbox = await communicationDomain.GetCommunicationDetailAsync(userCommunicationDetailQuery.Id, userCommunicationDetailQuery.Type, userCommunicationDetailQuery.UserId);

            return new UserCommunicationDetailEnvelop(communicationInbox);
        }
    }
}
