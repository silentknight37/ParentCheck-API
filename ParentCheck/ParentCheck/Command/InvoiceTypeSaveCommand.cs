using MediatR;
using ParentCheck.BusinessObject;
using ParentCheck.Envelope;
using System;
using System.Collections.Generic;

namespace ParentCheck.Query
{
    public class InvoiceTypeSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public InvoiceTypeSaveCommand(
            long id, 
            string typeText, 
            int terms, 
            bool isActive, 
            long userId)
        {
            this.Id = id;
            this.TypeText = typeText;
            this.Terms = terms;
            this.isActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public string TypeText { get; set; }
        public int Terms { get; set; }
        public bool isActive { get; set; }
        public long UserId { get; set; }
    }
}
