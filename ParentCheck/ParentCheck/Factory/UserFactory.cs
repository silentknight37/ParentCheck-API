using ParentCheck.Data;
using ParentCheck.Domain;
using ParentCheck.Factory.Intreface;
using ParentCheck.Repository;

namespace ParentCheck.Factory
{
    public class UserFactory : IUserFactory
    {
        private ParentcheckContext _parentCheckContext;

        public UserFactory(ParentcheckContext parentCheckContext)
        {
            _parentCheckContext = parentCheckContext;
        }

        IUserDomain IUserFactory.Create()
        {
            return new UserDomain(new UserRepository(_parentCheckContext));
        }
    }
}
