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
    public class UserQueryHandler:IRequestHandler<UserQuery, UserEnvelop>
    {
        private readonly IUserFactory userFactory;

        public UserQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.userFactory = new UserFactory(parentcheckContext);
        }

        public async Task<UserEnvelop> Handle(UserQuery userQuery,CancellationToken cancellationToken)
        {
            var userFactory = this.userFactory.Create();
            return new UserEnvelop();
        }
    }
}
