using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class InvoiceTypeDTO
    {
        public long Id { get; set; }
        public string InvoiceTypeText { get; set; }
        public int NumbersOfTerms { get; set; }
        public bool IsActive { get; set; }
    }
}
