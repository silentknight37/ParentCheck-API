using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class CommunicationDetailResponses
    {
        public CommunicationInbox message { get; set; }

        public static CommunicationDetailResponses PopulateCommunicationDetailResponses(CommunicationDTO communication)
        {
            var communicationDetailResponses = new CommunicationDetailResponses();
            communicationDetailResponses.message = new CommunicationInbox();

            if (communication != null)
            {
                communicationDetailResponses.message = new CommunicationInbox
                {
                    id = communication.Id,
                    subject = communication.Subject,
                    message = communication.Message,
                    type = communication.CommunicationType,
                    date = communication.SendDate,
                    fromUser = communication.FromUser,
                    toUser = communication.ToUser
                };
            }

            return communicationDetailResponses;
        }
    }
}
