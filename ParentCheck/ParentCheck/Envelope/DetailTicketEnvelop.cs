using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class DetailTicketEnvelop
    {
        public DetailTicketEnvelop(SupportTicketDTO supportTicket)
        {
            this.SupportTicket = supportTicket;
        }

        public SupportTicketDTO SupportTicket { get; set; }
    }
}
