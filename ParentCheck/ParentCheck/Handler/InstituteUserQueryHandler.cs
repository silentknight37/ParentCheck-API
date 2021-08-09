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
    public class InstituteUserQueryHandler : IRequestHandler<InstituteUsersQuery, InstituteUsersEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public InstituteUserQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<InstituteUsersEnvelop> Handle(InstituteUsersQuery instituteUsersQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var users = await settingDomain.GeInstituteUsers(instituteUsersQuery.UserId);
            return new InstituteUsersEnvelop(users);
        }
    }
}
