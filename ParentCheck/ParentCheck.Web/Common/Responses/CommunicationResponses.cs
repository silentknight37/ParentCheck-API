using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class CommunicationResponses
    {
        public List<CommunicationInbox> messages { get; set; }

        public static CommunicationResponses PopulateCommunicationResponses(List<CommunicationDTO> communications)
        {
            var communicationResponses = new CommunicationResponses();
            communicationResponses.messages = new List<CommunicationInbox>();

            foreach (var communication in communications)
            {
                var item = new CommunicationInbox
                {
                    id= communication.Id,
                    subject = communication.Subject,
                    message = communication.Message,
                    type = communication.CommunicationType,
                    date= communication.SendDate,
                    fromUser= communication.FromUser,
                    toUser= communication.ToUser,
                    templateId= communication.CommunicationTemplateId,
                    templateName= communication.CommunicationTemplateName,
                    templateContent= communication.CommunicationTemplateContent,
                };

                communicationResponses.messages.Add(item);
            }

            return communicationResponses;
        }
    }

    public class CommunicationInbox
    {
        public long id { get; set; }
        public string subject { get; set; }
        public int type { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }
        public string fromUser { get; set; }
        public long fromUserId { get; set; }
        public string toUser { get; set; }
        public long templateId { get; set; }
        public string templateName { get; set; }
        public string templateContent { get; set; }
    }
}
