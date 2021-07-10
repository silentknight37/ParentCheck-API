using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class SupportTicketConversationDTO
    {
        public long Id { get; set; }
        public long? SupportTicketId { get; set; }
        public string ConversationText { get; set; }
        public long? ReplyBy { get; set; }
        public string ReplyUser { get; set; }
        public DateTime? ReplyOn { get; set; }
        public string ImageUrl { get; set; }
    }
}
