using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class CommunicationDTO
    {
        public long Id { get; set; }
        public string Subject { get; set; }
        public int CommunicationType { get; set; }
        public string Message { get; set; }
        public DateTime SendDate { get; set; }
        public string FromUser { get; set; }
        public long FromUserId { get; set; }
        public string ToUser { get; set; }
        public long CommunicationTemplateId { get; set; }
        public string CommunicationTemplateName { get; set; }
        public string CommunicationTemplateContent { get; set; }
    }
}
