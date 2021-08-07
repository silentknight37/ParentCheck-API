using MediatR;
using ParentCheck.BusinessObject;
using ParentCheck.Envelope;
using System;
using System.Collections.Generic;

namespace ParentCheck.Query
{
    public class GenerateInvoiceCommand : IRequest<RequestSaveEnvelop>
    {
        public GenerateInvoiceCommand(
            string invoiceTitle, 
            string invoiceDetails, 
            List<UserContactDTO> toUsers,
            List<ReferenceDTO> toGroups, 
            bool isGroup, 
            DateTime dueDate, 
            DateTime invoiceDate, 
            decimal invoiceAmount,
            int invoiceTypeId, 
            long userId)
        {
            this.InvoiceTitle = invoiceTitle;
            this.InvoiceDetails = invoiceDetails;
            this.ToUsers = toUsers;
            this.ToGroups = toGroups;
            this.IsGroup = isGroup;
            this.DueDate = dueDate;
            this.InvoiceDate = invoiceDate;
            this.InvoiceAmount = invoiceAmount;
            this.InvoiceTypeId = invoiceTypeId;
            this.UserId = userId;
        }

        public string InvoiceTitle { get; set; }
        public string InvoiceDetails { get; set; }
        public List<UserContactDTO> ToUsers { get; set; }
        public List<ReferenceDTO> ToGroups { get; set; }
        public bool IsGroup { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public int InvoiceTypeId { get; set; }
        public long UserId { get; set; }
    }
}
