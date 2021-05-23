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
    public class PackageQueryHandler:IRequestHandler<PackageQuery, PackageEnvelop>
    {
        private readonly IPackageFactory packageFactory;

        public PackageQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.packageFactory = new PackageFactory(parentcheckContext);
        }

        public async Task<PackageEnvelop> Handle(PackageQuery packageQuery,CancellationToken cancellationToken)
        {
            var packageFactory = this.packageFactory.Create();
            return new PackageEnvelop();
        }
    }
}
