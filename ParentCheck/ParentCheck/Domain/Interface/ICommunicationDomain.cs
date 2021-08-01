using ParentCheck.BusinessObject;
using ParentCheck.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public interface ICommunicationDomain
    {
        Task<List<CommunicationDTO>> GetCommunicationInboxAsync(long userId);
        Task<List<CommunicationDTO>> GetCommunicationOutboxAsync(long userId);
        Task<List<CommunicationDTO>> GetCommunicationDetailAsync(long id, int type, long userId);
        Task<bool> SaveComposeCommunicationAsync(string subject, string messageText, List<UserContactDTO> toUsers, bool isGroup, DateTime? date, long? templateId, int communicationType, UserContactDTO fromUser, long userId);
        Task<UserContactDTO> GetFromUserCommunicationAsync(long userId);
        Task<List<UserContactDTO>> GetToUserCommunicationAsync(List<ReferenceDTO> toGroup, long userId);
        Task<List<CommunicationDTO>> GetSMSCommunicationOutboxAsync(long userId);
        Task<List<CommunicationTemplateDTO>> GetCommunicationTemplateAsync(bool isActiveOnly,long userId);
        Task<bool> SaveCommunicationTemplate(long id, string name, string content, bool isSenderTemplate, bool isActive, long userId);
        Task<bool> SaveReplyCommunicationAsync(long id, string subject, string messageText, long toUserId, UserContactDTO toUser, UserContactDTO fromUser, long userId);
    }
}
