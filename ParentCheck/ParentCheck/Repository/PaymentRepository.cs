using Microsoft.EntityFrameworkCore;
using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ParentCheck.Repository
{
    public class PaymentRepository: IPaymentRepository
    {
        private ParentCheckContext _parentcheckContext;

        public PaymentRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }

        public async Task<List<InvoiceDTO>> GetInvoiceAsync(long userId)
        {
            List<InvoiceDTO> invoiceDTOs = new List<InvoiceDTO>();

            var user = (from u in _parentcheckContext.User
                        join iu in _parentcheckContext.InstituteUser on u.Id equals iu.UserId
                        where iu.Id == userId
                        select new
                        {
                            u.FirstName,
                            u.LastName,
                            iu.Id,
                            iu.InstituteId,
                            iu
                        }).FirstOrDefault();

            if (user != null)
            {
                var invoices = await (from id in _parentcheckContext.InstituteInvoiceDetail
                                       join i in _parentcheckContext.InstituteInvoice on id.InvoiceId equals i.Id
                                       join s in _parentcheckContext.Status on i.StatusId equals s.Id
                                       join it in _parentcheckContext.InvoiceType on i.InvoiceTypeId equals it.Id
                                       where id.InstituteUserId==user.Id 
                                       && s.StatueTypeCode=="invoice"
                                       select new
                                       {
                                           i.Id,
                                           i.InvoiceAmount,
                                           i.InvoiceDate,
                                           i.InvoiceNo,
                                           i.InvoiceTitle,
                                           i.InvoiceDetails,
                                           i.DueDate,
                                           s.StatusText,
                                           it.InvoiceTypeText,
                                           id.PaidAmount,
                                           id.DueAmount
                                       }).ToListAsync();

                foreach (var invoice in invoices)
                {
                    invoiceDTOs.Add(new InvoiceDTO
                    {
                       Id= invoice.Id,
                       InvoiceNo= invoice.InvoiceNo,
                       InvoiceDate= invoice.InvoiceDate,
                       DueDate= invoice.DueDate,
                       InvoiceTitle= invoice.InvoiceTitle,
                       InvoiceDetails= invoice.InvoiceDetails,
                       InvoiceAmount= invoice.InvoiceAmount,
                       StatusText= invoice.StatusText,
                       InvoiceType=invoice.InvoiceTypeText,
                       DueAmount=invoice.DueAmount,
                       PaidAmount=invoice.PaidAmount
                    });
                }
            }

            return invoiceDTOs;
        }
    }
}
