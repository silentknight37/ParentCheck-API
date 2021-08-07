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
    public class UserAuthenticateQueryHandler : IRequestHandler<UserAuthenticateQuery, UserEnvelop>
    {
        private readonly IUserFactory userFactory;

        public UserAuthenticateQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.userFactory = new UserFactory(parentcheckContext);
        }

        public async Task<UserEnvelop> Handle(UserAuthenticateQuery userAuthenticateQuery,CancellationToken cancellationToken)
        {
            var userDomain = this.userFactory.Create();
            var user = await userDomain.GetUserAuthenticateAsync(userAuthenticateQuery.Username, userAuthenticateQuery.Password);
            return new UserEnvelop(user);
        }
    }
}
