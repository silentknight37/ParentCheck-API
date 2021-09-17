using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class InvoiceGenerateRequest
    {
        public string invoiceTitle { get; set; }
        public string invoiceDetails { get; set; }
        public List<UserContactRequest> toUsers { get; set; }
        public List<ReferenceRequest> toGroups { get; set; }
        public bool isGroup { get; set; }
        public string dueDate { get; set; }
        public string invoiceDate { get; set; }
        public decimal invoiceAmount { get; set; }
        public int invoiceTypeId { get; set; }
    }
}
