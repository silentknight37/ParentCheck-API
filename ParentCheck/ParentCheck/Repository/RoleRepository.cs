using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Repository
{
    public class RoleRepository: IRoleRepository
    {
        private ParentcheckContext _parentcheckContext;

        public RoleRepository(ParentcheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }
    }
}
