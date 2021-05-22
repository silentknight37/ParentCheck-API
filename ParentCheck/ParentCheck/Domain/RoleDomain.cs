using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Domain
{
    public class RoleDomain: IRoleDomain
    {
        private readonly IRoleRepository _roleRepository;

        public RoleDomain(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
    }
}
