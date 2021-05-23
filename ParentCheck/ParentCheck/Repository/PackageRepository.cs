using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Repository
{
    public class PackageRepository: IPackageRepository
    {
        private ParentCheckContext _parentcheckContext;

        public PackageRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }
    }
}
