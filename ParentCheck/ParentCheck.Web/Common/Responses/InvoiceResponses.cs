using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class InvoiceResponses
    {
        public List<Invoice> invoices { get; set; }

        public static InvoiceResponses PopulateInvoiceResponses(List<InvoiceDTO> invoiceDTOs)
        {
            var invoiceResponses = new InvoiceResponses();
            invoiceResponses.invoices = new List<Invoice>();

            foreach (var invoiceDTO in invoiceDTOs)
            {
                var invoice = new Invoice
                {
                    invoiceNo = invoiceDTO.InvoiceNo,
                    invoiceDate= invoiceDTO.InvoiceDate,
                    dueDate= invoiceDTO.DueDate,
                    invoiceAmount= invoiceDTO.InvoiceAmount,
                    invoiceTitle= invoiceDTO.InvoiceTitle,
                    invoiceDetails= invoiceDTO.InvoiceDetails,
                    status= invoiceDTO.StatusText,
                    invoiceType=invoiceDTO.InvoiceType
                };

                invoiceResponses.invoices.Add(invoice);
            }

            return invoiceResponses;
        }
    }

    public class Invoice
    {
        public string invoiceNo { get; set; }
        public DateTime invoiceDate { get; set; }
        public DateTime dueDate { get; set; }
        public string invoiceTitle { get; set; }
        public string invoiceDetails { get; set; }
        public string status { get; set; }
        public string invoiceType { get; set; }
        public decimal invoiceAmount { get; set; }
    }
}
