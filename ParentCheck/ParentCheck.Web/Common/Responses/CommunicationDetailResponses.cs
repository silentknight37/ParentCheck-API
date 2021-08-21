using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class CommunicationDetailResponses
    {
        public List<CommunicationInbox> messages { get; set; }

        public static CommunicationDetailResponses PopulateCommunicationDetailResponses(List<CommunicationDTO> communications)
        {
            var communicationDetailResponses = new CommunicationDetailResponses();
            communicationDetailResponses.messages = new List<CommunicationInbox>();

            foreach (var communication in communications)
            {
                communicationDetailResponses.messages.Add(new CommunicationInbox
                {
                    id = communication.Id,
                    subject = communication.Subject,
                    message = communication.Message,
                    type = communication.CommunicationType,
                    date = communication.SendDate.ToString("dd/MM/yyyy"),
                    fromUser = communication.FromUser,
                    fromUserId = communication.FromUserId,
                    toUser = communication.ToUser,
                    templateContent = communication.CommunicationTemplateContent,
                    templateId = communication.CommunicationTemplateId,
                    templateName = communication.CommunicationTemplateName
                });
            }

            return communicationDetailResponses;
        }
    }
}
