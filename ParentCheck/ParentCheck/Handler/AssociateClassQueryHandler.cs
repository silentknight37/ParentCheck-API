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
    public class AssociateClassQueryHandler : IRequestHandler<AssociateClassQuery, AssociateClassEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public AssociateClassQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<AssociateClassEnvelop> Handle(AssociateClassQuery associateClassQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var associateClasses = await settingDomain.GetAssociateClass(associateClassQuery.UserId);
            return new AssociateClassEnvelop(associateClasses);
        }
    }
}
