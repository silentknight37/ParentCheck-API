using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class SupportTicketDTO
    {
        public SupportTicketDTO()
        {
            SupportTicketConversations = new List<SupportTicketConversationDTO>();
        }

        public long Id { get; set; }
        public string Subject { get; set; }
        public string IssueText { get; set; }
        public string StatusText { get; set; }
        public int? StatusId { get; set; }
        public string RaisedBy { get; set; }
        public DateTime? OpenDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public List<SupportTicketConversationDTO> SupportTicketConversations { get; set; }
    }
}
