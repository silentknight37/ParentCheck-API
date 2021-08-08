using ParentCheck.BusinessObject;
using ParentCheck.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository.Intreface
{
    public interface ISettingRepository
    {
        Task<bool> NewSupportTicketAsync(string subject, string issueText, long userId);
        Task<List<SupportTicketDTO>> GetTicketsAsync(EnumSupportTicketType ticketType, long userId);
        Task<SupportTicketDTO> GetDetailTicketsAsync(long id, long userId);
        Task<bool> ReplySupportTicketAsync(long ticketId, string replyMessage, long userId);
    }
}
