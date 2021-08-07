using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository.Intreface
{
    public interface IPaymentRepository
    {
        Task<List<InvoiceDTO>> GetInvoiceAsync(bool isGenerated, long userId);
        Task<InvoiceDTO> GetInvoiceDetailAsync(bool isGenerated, long invoiceId, long userId);
        Task<bool> GenerateInvoiceAsync(string invoiceTitle, string invoiceDetails, List<UserContactDTO> toUsers, DateTime dueDate, DateTime invoiceDate, decimal invoiceAmount, int invoiceTypeId, long userId);
        Task<List<InvoiceTypeDTO>> GetInvoiceTypesAsync(long userId);
        Task<bool> InvoiceTypeSaveAsync(long id, string typeText, int terms, bool isActive, long userId);
    }
}
