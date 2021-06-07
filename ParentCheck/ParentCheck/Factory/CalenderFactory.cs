using ParentCheck.Data;
using ParentCheck.Domain;
using ParentCheck.Factory.Intreface;
using ParentCheck.Repository;

namespace ParentCheck.Factory
{
    public class CalenderFactory : ICalenderFactory
    {
        private ParentCheckContext _parentCheckContext;

        public CalenderFactory(ParentCheckContext parentCheckContext)
        {
            _parentCheckContext = parentCheckContext;
        }

        ICalenderDomain ICalenderFactory.Create()
        {
            return new CalenderDomain(new CalenderRepository(_parentCheckContext));
        }
    }
}
