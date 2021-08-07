using ParentCheck.BusinessObject;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public class PaymentDomain: IPaymentDomain
    {
        private readonly IPaymentRepository _paymentRepository;

        public PaymentDomain(IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
        }

        public async Task<List<InvoiceDTO>> GetInvoiceAsync(bool isGenerated,long userId)
        {
            return await _paymentRepository.GetInvoiceAsync(isGenerated,userId);
        }

        public async Task<InvoiceDTO> GetInvoiceDetailAsync(bool isGenerated, long invoiceId,long userId)
        {
            return await _paymentRepository.GetInvoiceDetailAsync(isGenerated,invoiceId, userId);
        }
        public async Task<bool> GenerateInvoiceAsync(string invoiceTitle, string invoiceDetails, List<UserContactDTO> toUsers, DateTime dueDate, DateTime invoiceDate, decimal invoiceAmount, int invoiceTypeId, long userId)
        {
            return await _paymentRepository.GenerateInvoiceAsync(invoiceTitle, invoiceDetails, toUsers, dueDate, invoiceDate, invoiceAmount, invoiceTypeId, userId);
        }

        public async Task<List<InvoiceTypeDTO>> GetInvoiceTypesAsync(long userId)
        {
            return await _paymentRepository.GetInvoiceTypesAsync(userId);
        }

        public async Task<bool> InvoiceTypeSaveAsync(long id, string typeText, int terms, bool isActive, long userId)
        {
            return await _paymentRepository.InvoiceTypeSaveAsync(id, typeText, terms, isActive, userId);
        }
    }
}
