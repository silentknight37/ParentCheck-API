using ParentCheck.BusinessObject;
using ParentCheck.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public interface ISettingDomain
    {
        Task<bool> NewSupportTicketAsync(string subject, string description, long userId);
        Task<List<SupportTicketDTO>> GetTicketsAsync(EnumSupportTicketType ticketType, long userId);
        Task<SupportTicketDTO> GetDetailTicketsAsync(long id, long userId);
        Task<bool> SupportTicketReplyAsync(long ticketId, string replyMessage, long userId);
    }
}
