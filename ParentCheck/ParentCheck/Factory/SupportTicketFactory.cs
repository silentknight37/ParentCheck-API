using ParentCheck.Data;
using ParentCheck.Domain;
using ParentCheck.Factory.Intreface;
using ParentCheck.Repository;

namespace ParentCheck.Factory
{
    public class SupportTicketFactory : ISupportTicketFactory
    {
        private ParentCheckContext _parentCheckContext;

        public SupportTicketFactory(ParentCheckContext parentCheckContext)
        {
            _parentCheckContext = parentCheckContext;
        }

        ISupportTicketDomain ISupportTicketFactory.Create()
        {
            return new SupportTicketDomain(new SupportTicketRepository(_parentCheckContext));
        }
    }
}
