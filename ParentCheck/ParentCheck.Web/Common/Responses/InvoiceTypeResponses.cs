using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class InvoiceTypeResponses
    {
        public List<InvoiceType> invoiceTypes { get; set; }

        public static InvoiceTypeResponses PopulateInvoiceTypeResponses(List<InvoiceTypeDTO> invoiceDTOs)
        {
            var invoiceTypeResponses = new InvoiceTypeResponses();
            invoiceTypeResponses.invoiceTypes = new List<InvoiceType>();

            foreach (var invoiceDTO in invoiceDTOs)
            {
                var invoiceType = new InvoiceType
                {
                    id= invoiceDTO.Id,
                    invoiceType= invoiceDTO.InvoiceTypeText,
                    terms= invoiceDTO.NumbersOfTerms,
                    isActive=invoiceDTO.IsActive
                };

                invoiceTypeResponses.invoiceTypes.Add(invoiceType);
            }

            return invoiceTypeResponses;
        }
    }

    public class InvoiceType
    {
        public long id { get; set; }
        public string invoiceType { get; set; }
        public int terms { get; set; }
        public bool isActive { get; set; }
    }
}
