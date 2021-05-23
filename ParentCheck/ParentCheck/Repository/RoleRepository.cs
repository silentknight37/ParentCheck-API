using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Repository
{
    public class RoleRepository: IRoleRepository
    {
        private ParentCheckContext _parentcheckContext;

        public RoleRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }
    }
}
