using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class InvoiceTypeEnvelop
    {
        public InvoiceTypeEnvelop(List<InvoiceTypeDTO> invoiceTypeDTOs)
        {
            this.InvoiceTypes = invoiceTypeDTOs;
        }

        public List<InvoiceTypeDTO> InvoiceTypes { get; set; }
    }
}
