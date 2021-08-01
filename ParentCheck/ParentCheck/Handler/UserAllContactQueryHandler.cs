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
    public class UserAllContactQueryHandler : IRequestHandler<UserAllContactQuery, UserContactEnvelop>
    {
        private readonly IReferenceFactory referenceFactory;

        public UserAllContactQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.referenceFactory = new ReferenceFactory(parentcheckContext);
        }

        public async Task<UserContactEnvelop> Handle(UserAllContactQuery userAllContactQuery,CancellationToken cancellationToken)
        {
            var referenceDomain = this.referenceFactory.Create();
            var userContact = await referenceDomain.GetAllUserContactAsync(userAllContactQuery.SendType,userAllContactQuery.UserId);

            return new UserContactEnvelop(userContact);
        }
    }
}
