using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class SupportTicketResponses
    {
        public List<SupportTicket> supportTickets { get; set; }

        public static SupportTicketResponses PopulateSupportTicketResponses(List<SupportTicketDTO> supportTicketDTOs)
        {
            var supportTicketResponses = new SupportTicketResponses();
            supportTicketResponses.supportTickets = new List<SupportTicket>();

            foreach (var supportTicket in supportTicketDTOs)
            {
                var support = new SupportTicket
                {
                    id= supportTicket.Id,
                    subject = supportTicket.Subject,
                    description = supportTicket.IssueText,
                    status = supportTicket.StatusText,
                    openDate=supportTicket.OpenDate,
                    closeDate=supportTicket.LastUpdateDate
                };

                supportTicketResponses.supportTickets.Add(support);
            }

            return supportTicketResponses;
        }
    }

    public class SupportTicket
    {
        public SupportTicket()
        {
            supportTicketConversations = new List<SupportTicketConversation>();
        }

        public long id { get; set; }
        public string subject { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public DateTime? openDate { get; set; }
        public DateTime? closeDate { get; set; }
        public string openBy { get; set; }
        public List<SupportTicketConversation> supportTicketConversations { get; set; }
    }
}
