using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class InvoiceEnvelop
    {
        public InvoiceEnvelop(List<InvoiceDTO> invoiceDTOs)
        {
            this.Invoices = invoiceDTOs;
        }

        public List<InvoiceDTO> Invoices { get; set; }
    }
}
