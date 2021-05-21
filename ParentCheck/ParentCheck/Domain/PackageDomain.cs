using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Domain
{
    public class PackageDomain: IPackageDomain
    {
        private readonly IPackageRepository _packageRepository;

        public PackageDomain(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

    }
}
