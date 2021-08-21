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
                    id= invoiceDTO.Id,
                    invoiceNo = invoiceDTO.InvoiceNo,
                    invoiceDate= invoiceDTO.InvoiceDate.ToString("dd/MM/yyyy"),
                    dueDate= invoiceDTO.DueDate.ToString("dd/MM/yyyy"),
                    invoiceAmount= invoiceDTO.InvoiceAmount,
                    invoiceTitle= invoiceDTO.InvoiceTitle,
                    invoiceDetails= invoiceDTO.InvoiceDetails,
                    status= invoiceDTO.StatusText,
                    invoiceType=invoiceDTO.InvoiceType,
                    invoiceTo=invoiceDTO.InvoiceUserName
                };

                invoiceResponses.invoices.Add(invoice);
            }

            return invoiceResponses;
        }
    }

    public class Invoice
    {
        public long id { get; set; }
        public string invoiceNo { get; set; }
        public string invoiceDate { get; set; }
        public string dueDate { get; set; }
        public string invoiceTitle { get; set; }
        public string invoiceDetails { get; set; }
        public string status { get; set; }
        public string invoiceType { get; set; }
        public decimal invoiceAmount { get; set; }
        public decimal dueAmount { get; set; }
        public decimal payAmount { get; set; }
        public string invoiceTo { get; set; }
    }
}
