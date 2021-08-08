using ParentCheck.Data;
using ParentCheck.Domain;
using ParentCheck.Factory.Intreface;
using ParentCheck.Repository;

namespace ParentCheck.Factory
{
    public class SettingFactory : ISettingFactory
    {
        private ParentCheckContext _parentCheckContext;

        public SettingFactory(ParentCheckContext parentCheckContext)
        {
            _parentCheckContext = parentCheckContext;
        }

        ISettingDomain ISettingFactory.Create()
        {
            return new SettingDomain(new SettingRepository(_parentCheckContext));
        }
    }
}
