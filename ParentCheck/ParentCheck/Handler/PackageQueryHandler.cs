using MediatR;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Factory;
using ParentCheck.Factory.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParentCheck.Handler
{
    public class PackageQueryHandler:IRequestHandler<PackageQuery, PackageEnvelop>
    {
        private readonly IPackageFactory packageFactory;

        public PackageQueryHandler(ParentcheckContext parentcheckContext)
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
