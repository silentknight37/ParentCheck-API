using ParentCheck.Data;
using ParentCheck.Domain;
using ParentCheck.Factory.Intreface;
using ParentCheck.Repository;

namespace ParentCheck.Factory
{
    public class ReferenceFactory : IReferenceFactory
    {
        private ParentCheckContext _parentCheckContext;

        public ReferenceFactory(ParentCheckContext parentCheckContext)
        {
            _parentCheckContext = parentCheckContext;
        }

        IReferenceDomain IReferenceFactory.Create()
        {
            return new ReferenceDomain(new ReferenceRepository(_parentCheckContext));
        }
    }
}
