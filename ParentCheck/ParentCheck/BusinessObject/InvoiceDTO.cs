using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class InvoiceDTO
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public string InvoiceTitle { get; set; }
        public string InvoiceDetails { get; set; }
        public string StatusText { get; set; }
        public int StatusId { get; set; }
        public string InvoiceType { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }
        public long GeneratedBy { get; set; }
        public string GeneratedUserName { get; set; }
        public string InvoiceUserName { get; set; }
    }
}
