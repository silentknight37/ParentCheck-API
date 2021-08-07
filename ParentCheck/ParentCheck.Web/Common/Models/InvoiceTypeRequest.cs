using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class InvoiceTypeRequest
    {
        public long Id { get; set; }
        public string TypeText { get; set; }
        public int Terms { get; set; }
        public bool IsActive { get; set; }
    }
}
