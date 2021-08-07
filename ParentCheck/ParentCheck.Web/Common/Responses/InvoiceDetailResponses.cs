using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class InvoiceDetailResponses
    {
        public Invoice invoice { get; set; }

        public static InvoiceDetailResponses PopulateInvoiceDetailResponses(InvoiceDTO invoiceDTO)
        {
            var invoiceDetailResponses = new InvoiceDetailResponses();
            invoiceDetailResponses.invoice = new Invoice
            {
                invoiceNo = invoiceDTO.InvoiceNo,
                invoiceDate = invoiceDTO.InvoiceDate,
                dueDate = invoiceDTO.DueDate,
                invoiceAmount = invoiceDTO.InvoiceAmount,
                invoiceTitle = invoiceDTO.InvoiceTitle,
                invoiceDetails = invoiceDTO.InvoiceDetails,
                status = invoiceDTO.StatusText,
                invoiceType = invoiceDTO.InvoiceType,
                invoiceTo = invoiceDTO.InvoiceUserName,
                dueAmount= invoiceDTO.DueAmount,
                payAmount= invoiceDTO.PaidAmount
            };

            return invoiceDetailResponses;
        }
    }
}
