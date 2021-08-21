using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class CommunicationTemplateResponses
    {
        public List<CommunicationTemplate> templates { get; set; }

        public static CommunicationTemplateResponses PopulateCommunicationTemplateResponseResponses(List<CommunicationTemplateDTO> communicationTemplates)
        {
            var communicationTemplateResponses = new CommunicationTemplateResponses();
            communicationTemplateResponses.templates = new List<CommunicationTemplate>();

            foreach (var communicationTemplate in communicationTemplates)
            {
                var item = new CommunicationTemplate
                {
                    id= communicationTemplate.Id,
                    name= communicationTemplate.TemplateName,
                    content= communicationTemplate.TemplateContent,
                    isSenderTemplate= communicationTemplate.IsSenderTemplate,
                    isActive= communicationTemplate.IsActive,
                    lastUpdatedBy= communicationTemplate.LastUpdatedBy,
                };

                communicationTemplateResponses.templates.Add(item);
            }

            return communicationTemplateResponses;
        }
    }

    public class CommunicationTemplate
    {
        public long id { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public bool isSenderTemplate { get; set; }
        public bool isActive { get; set; }
        public string lastUpdatedBy { get; set; }
    }
}
