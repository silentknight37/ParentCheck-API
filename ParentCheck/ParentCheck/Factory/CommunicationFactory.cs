using ParentCheck.Data;
using ParentCheck.Domain;
using ParentCheck.Factory.Intreface;
using ParentCheck.Repository;

namespace ParentCheck.Factory
{
    public class CommunicationFactory : ICommunicationFactory
    {
        private ParentCheckContext _parentCheckContext;

        public CommunicationFactory(ParentCheckContext parentCheckContext)
        {
            _parentCheckContext = parentCheckContext;
        }

        ICommunicationDomain ICommunicationFactory.Create()
        {
            return new CommunicationDomain(new CommunicationRepository(_parentCheckContext));
        }
    }
}
