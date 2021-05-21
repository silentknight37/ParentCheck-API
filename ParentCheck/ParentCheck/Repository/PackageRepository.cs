using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Repository
{
    public class PackageRepository: IPackageRepository
    {
        private ParentcheckContext _parentcheckContext;

        public PackageRepository(ParentcheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }
    }
}
