using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public class SupportTicketDomain : ISupportTicketDomain
    {
        private readonly ISupportTicketRepository supportTicketRepository;

        public SupportTicketDomain(ISupportTicketRepository supportTicketRepository)
        {
            this.supportTicketRepository = supportTicketRepository;
        }

        public async Task<bool> NewSupportTicketAsync(string subject, string issueText, long userId)
        {
            return await supportTicketRepository.NewSupportTicketAsync(subject, issueText, userId);
        }

        public async Task<List<SupportTicketDTO>> GetTicketsAsync(EnumSupportTicketType ticketType, long userId)
        {
            return await supportTicketRepository.GetTicketsAsync(ticketType,userId);
        }

        public async Task<SupportTicketDTO> GetDetailTicketsAsync(long id, long userId)
        {
            return await supportTicketRepository.GetDetailTicketsAsync(id, userId);
        }

        public async Task<bool> SupportTicketReplyAsync(long ticketId, string replyMessage, long userId)
        {
            return await supportTicketRepository.ReplySupportTicketAsync(ticketId, replyMessage, userId);
        }
    }
}
