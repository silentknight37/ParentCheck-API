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
    public class UserContactQueryHandler : IRequestHandler<UserContactQuery, UserContactEnvelop>
    {
        private readonly IReferenceFactory referenceFactory;

        public UserContactQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.referenceFactory = new ReferenceFactory(parentcheckContext);
        }

        public async Task<UserContactEnvelop> Handle(UserContactQuery userContactQuery,CancellationToken cancellationToken)
        {
            var referenceDomain = this.referenceFactory.Create();
            var userContact = await referenceDomain.GetUserContactAsync(userContactQuery.Name, userContactQuery.UserId);

            return new UserContactEnvelop(userContact);
        }
    }
}
