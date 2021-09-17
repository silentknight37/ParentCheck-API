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
    public class UserStudentsContactQueryHandler : IRequestHandler<UserStudentsContactQuery, UserContactEnvelop>
    {
        private readonly IReferenceFactory referenceFactory;

        public UserStudentsContactQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.referenceFactory = new ReferenceFactory(parentcheckContext);
        }

        public async Task<UserContactEnvelop> Handle(UserStudentsContactQuery userStudentsContactQuery,CancellationToken cancellationToken)
        {
            var referenceDomain = this.referenceFactory.Create();
            var userContact = await referenceDomain.GetStudentUserContactAsync(userStudentsContactQuery.SendType, userStudentsContactQuery.UserId);

            return new UserContactEnvelop(userContact); 
        }
    }
}
