using ParentCheck.BusinessObject;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public interface IPaymentDomain
    {
        Task<List<InvoiceDTO>> GetInvoiceAsync(long userId);
    }
}
