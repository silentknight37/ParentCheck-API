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

        public async Task<List<InvoiceDTO>> GetInvoiceAsync(long userId)
        {
            return await _paymentRepository.GetInvoiceAsync(userId);
        }
    }
}
