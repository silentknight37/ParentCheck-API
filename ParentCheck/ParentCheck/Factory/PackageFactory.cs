using ParentCheck.Data;
using ParentCheck.Domain;
using ParentCheck.Factory.Intreface;
using ParentCheck.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Factory
{
    public class PackageFactory : IPackageFactory
    {
        private ParentcheckContext _parentCheckContext;

        public PackageFactory(ParentcheckContext parentCheckContext)
        {
            _parentCheckContext = parentCheckContext;
        }

        IPackageDomain IPackageFactory.Create()
        {
            return new PackageDomain(new PackageRepository(_parentCheckContext));
        }
    }
}
