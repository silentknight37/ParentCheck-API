using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class CommunicationTemplateEnvelop
    {
        public CommunicationTemplateEnvelop(List<CommunicationTemplateDTO> communicationTemplateDTOs)
        {
            this.CommunicationTemplates = communicationTemplateDTOs;
        }

        public List<CommunicationTemplateDTO> CommunicationTemplates { get; set; }
    }
}
