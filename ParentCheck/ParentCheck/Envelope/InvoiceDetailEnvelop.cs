using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class InvoiceDetailEnvelop
    {
        public InvoiceDetailEnvelop(InvoiceDTO invoiceDTO)
        {
            this.Invoice = invoiceDTO;
        }

        public InvoiceDTO Invoice { get; set; }
    }
}
