using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class CommunicationTemplateDTO
    {
        public long Id { get; set; }
        public string TemplateName { get; set; }
        public string TemplateContent { get; set; }
        public bool IsSenderTemplate { get; set; }
        public bool IsActive { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
