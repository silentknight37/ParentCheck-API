using Microsoft.EntityFrameworkCore;
using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository
{
    public class SupportTicketRepository : ISupportTicketRepository
    {
        private ParentCheckContext _parentcheckContext;

        public SupportTicketRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }

        public async Task<bool> NewSupportTicketAsync(string subject, string issueText, long userId)
        {
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                SupportTicket supportTicket = new SupportTicket();
                supportTicket.Subject = subject;
                supportTicket.IssueText = issueText;
                supportTicket.StatusId = await GetStatus(user.u);
                supportTicket.RaisedBy = user.Id;
                supportTicket.RaisedOn = DateTime.Now;
                supportTicket.AssignedUserId = await GetAssignedUser(user.u);
                supportTicket.InstituteId = user.InstituteId;
                supportTicket.CreatedOn = DateTime.UtcNow;
                supportTicket.CreatedBy = $"{user.FirstName} {user.LastName}";
                supportTicket.UpdateOn = DateTime.UtcNow;
                supportTicket.UpdatedBy = $"{user.FirstName} {user.LastName}";

                _parentcheckContext.SupportTicket.Add(supportTicket);
                await _parentcheckContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        private async Task<long?> GetAssignedUser(InstituteUser instituteUser)
        {
            var userClass = (from uc in _parentcheckContext.InstituteUserClass
                             join c in _parentcheckContext.InstituteClass on uc.InstituteClassId equals c.Id
                             select c).FirstOrDefault();
            if (userClass != null)
            {
                return userClass.ResponsibleUserId;
            }

            return null;
        }

        private async Task<int> GetStatus(InstituteUser instituteUser)
        {
            var userClass = (from uc in _parentcheckContext.InstituteUserClass
                             join c in _parentcheckContext.InstituteClass on uc.InstituteClassId equals c.Id
                             select c).FirstOrDefault();
            if (userClass != null)
            {
                return userClass.ResponsibleUserId > 0 ? (int)EnumStatus.InReview : (int)EnumStatus.Open;
            }

            return (int)EnumStatus.Open;
        }

        public async Task<List<SupportTicketDTO>> GetTicketsAsync(EnumSupportTicketType ticketType, long userId)
        {
            List<SupportTicketDTO> supportTickets = new List<SupportTicketDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                if (EnumSupportTicketType.Open == ticketType)
                {
                    var calenderEventList = await (from st in _parentcheckContext.SupportTicket
                                                   join s in _parentcheckContext.Status on st.StatusId equals s.Id
                                                   where st.RaisedBy == user.Id && (st.StatusId == (int)EnumStatus.Open || st.StatusId == (int)EnumStatus.InProgress || st.StatusId == (int)EnumStatus.InReview)
                                                   select new SupportTicketDTO
                                                   {
                                                       Id = st.Id,
                                                       OpenDate = st.RaisedOn,
                                                       Subject = st.Subject,
                                                       IssueText = st.IssueText,
                                                       StatusText = s.StatusText,
                                                       StatusId = st.StatusId,
                                                       LastUpdateDate = st.UpdateOn
                                                   }).ToListAsync();

                    supportTickets.AddRange(calenderEventList);
                }

                if (EnumSupportTicketType.Closed == ticketType)
                {
                    var calenderEventList = await (from st in _parentcheckContext.SupportTicket
                                                   join s in _parentcheckContext.Status on st.StatusId equals s.Id
                                                   where st.RaisedBy == user.Id && st.StatusId == (int)EnumStatus.Closed
                                                   select new SupportTicketDTO
                                                   {
                                                       Id = st.Id,
                                                       OpenDate = st.RaisedOn,
                                                       Subject = st.Subject,
                                                       IssueText = st.IssueText,
                                                       StatusText = s.StatusText,
                                                       StatusId = st.StatusId,
                                                       LastUpdateDate = st.UpdateOn
                                                   }).ToListAsync();

                    supportTickets.AddRange(calenderEventList);
                }

                if (EnumSupportTicketType.Review == ticketType)
                {
                    var calenderEventList = await (from st in _parentcheckContext.SupportTicket
                                                   join s in _parentcheckContext.Status on st.StatusId equals s.Id
                                                   where st.AssignedUserId == user.Id && st.StatusId == (int)EnumStatus.InReview
                                                   select new SupportTicketDTO
                                                   {
                                                       Id = st.Id,
                                                       OpenDate = st.RaisedOn,
                                                       Subject = st.Subject,
                                                       IssueText = st.IssueText,
                                                       StatusText = s.StatusText,
                                                       StatusId = st.StatusId,
                                                       LastUpdateDate = st.UpdateOn
                                                   }).ToListAsync();

                    supportTickets.AddRange(calenderEventList);
                }
            }
            
            return supportTickets;
        }


        public async Task<SupportTicketDTO> GetDetailTicketsAsync(long id, long userId)
        {
            SupportTicketDTO supportTicket = new SupportTicketDTO();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                supportTicket = await (from st in _parentcheckContext.SupportTicket
                                       join s in _parentcheckContext.Status on st.StatusId equals s.Id
                                       where st.RaisedBy == user.Id && st.Id == id
                                       select new SupportTicketDTO
                                       {
                                           Id = st.Id,
                                           OpenDate = st.RaisedOn,
                                           Subject = st.Subject,
                                           IssueText = st.IssueText,
                                           StatusText = s.StatusText,
                                           StatusId = st.StatusId,
                                           RaisedBy = $"{user.FirstName} {user.LastName}",
                                           LastUpdateDate = st.UpdateOn
                                       }).FirstOrDefaultAsync();

                supportTicket.SupportTicketConversations.AddRange(await GetSupportTicketConversations(id));
            }

            return supportTicket;
        }

        private async Task<List<SupportTicketConversationDTO>> GetSupportTicketConversations(long id)
        {
            List<SupportTicketConversationDTO> supportTicketConversationList = new List<SupportTicketConversationDTO>();
            var supportTicketConversations = await _parentcheckContext.SupportTicketConversation.Where(i => i.SupportTicketId == id).ToListAsync();

            foreach (var supportTicketConversation in supportTicketConversations)
            {
                var conversation = new SupportTicketConversationDTO
                {
                    Id = supportTicketConversation.Id,
                    ConversationText = supportTicketConversation.ConversationText,
                    ReplyBy = supportTicketConversation.ReplyBy,
                    ReplyOn = supportTicketConversation.ReplyOn,
                    SupportTicketId = supportTicketConversation.SupportTicketId
                };

                await GetRepliedUser(conversation,supportTicketConversation);

                supportTicketConversationList.Add(conversation);
            }

            return supportTicketConversationList;
        }

        private async Task GetRepliedUser(SupportTicketConversationDTO conversation,SupportTicketConversation supportTicketConversation)
        {
            if (supportTicketConversation.IsAdminReply)
            {
                var adminUser = await _parentcheckContext.User.Where(i => i.Id == supportTicketConversation.ReplyBy).FirstOrDefaultAsync();

                conversation.ImageUrl = adminUser.ImageUrl;
                conversation.ReplyUser = $"{adminUser.FirstName} {adminUser.LastName}";
            }

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == supportTicketConversation.ReplyBy
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u.ImageUrl,
                                  u
                              }).FirstOrDefaultAsync();

            conversation.ImageUrl = user.ImageUrl;
            conversation.ReplyUser = $"{user.FirstName} {user.LastName}";
        }

        public async Task<bool> ReplySupportTicketAsync(long ticketId, string replyMessage, long userId)
        {
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                SupportTicketConversation supportTicketConversation = new SupportTicketConversation();
                supportTicketConversation.SupportTicketId = ticketId;
                supportTicketConversation.ConversationText = replyMessage;
                supportTicketConversation.ReplyBy = user.Id;
                supportTicketConversation.ReplyOn = DateTime.Now;
                supportTicketConversation.IsAdminReply = false;
                supportTicketConversation.CreatedOn = DateTime.UtcNow;
                supportTicketConversation.CreatedBy = $"{user.FirstName} {user.LastName}";
                supportTicketConversation.UpdateOn = DateTime.UtcNow;
                supportTicketConversation.UpdatedBy = $"{user.FirstName} {user.LastName}";

                _parentcheckContext.SupportTicketConversation.Add(supportTicketConversation);
                await _parentcheckContext.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
