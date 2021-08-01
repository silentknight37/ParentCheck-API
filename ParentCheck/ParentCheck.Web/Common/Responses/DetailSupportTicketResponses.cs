using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class DetailSupportTicketResponses
    {
        public SupportTicket detailTicket { get; set; }

        public static DetailSupportTicketResponses PopulateDetailSupportTicketResponses(long userId,SupportTicketDTO supportTicket)
        {
            var detailSupportTicketResponses = new DetailSupportTicketResponses();
            detailSupportTicketResponses.detailTicket = new SupportTicket();

            var support = new SupportTicket
            {
                id = supportTicket.Id,
                subject = supportTicket.Subject,
                description = supportTicket.IssueText,
                status = supportTicket.StatusText,
                openDate = supportTicket.OpenDate,
                closeDate = supportTicket.LastUpdateDate,
                openBy=supportTicket.RaisedBy
            };

            supportTicket.SupportTicketConversations.ForEach(i => support.supportTicketConversations.Add(
                new SupportTicketConversation()
                {
                    id = i.Id,
                    conversationText = i.ConversationText,
                    replyBy = i.ReplyBy,
                    replyOn = i.ReplyOn,
                    replyUser = i.ReplyUser,
                    imageUrl=i.ImageUrl,
                    isUserReply= i.ReplyBy.Value==userId
                }));


            detailSupportTicketResponses.detailTicket = support;

            return detailSupportTicketResponses;
        }
    }

    public class SupportTicketConversation
    {
        public long id { get; set; }
        public long? supportTicketId { get; set; }
        public string conversationText { get; set; }
        public long? replyBy { get; set; }
        public string replyUser { get; set; }
        public DateTime? replyOn { get; set; }
        public bool isUserReply { get; set; }
        public string imageUrl { get; set; }
    }
}
