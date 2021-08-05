using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository.Intreface
{
    public interface IPaymentRepository
    {
        Task<List<InvoiceDTO>> GetInvoiceAsync(long userId);
    }
}
