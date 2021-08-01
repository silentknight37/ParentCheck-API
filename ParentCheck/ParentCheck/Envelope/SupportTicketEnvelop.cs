using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class SupportTicketEnvelop
    {
        public SupportTicketEnvelop(List<SupportTicketDTO> supportTickets)
        {
            this.SupportTickets = supportTickets;
        }

        public List<SupportTicketDTO> SupportTickets { get; set; }
    }
}
