using ParentCheck.Data;
using ParentCheck.Domain;
using ParentCheck.Factory.Intreface;
using ParentCheck.Repository;

namespace ParentCheck.Factory
{
    public class RoleFactory : IRoleFactory
    {
        private ParentCheckContext _parentCheckContext;

        public RoleFactory(ParentCheckContext parentCheckContext)
        {
            _parentCheckContext = parentCheckContext;
        }

        IRoleDomain IRoleFactory.Create()
        {
            return new RoleDomain(new RoleRepository(_parentCheckContext));
        }
    }
}
