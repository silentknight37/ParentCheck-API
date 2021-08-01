using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public class CommunicationDomain : ICommunicationDomain
    {
        private readonly ICommunicationRepository communicationRepository;

        public CommunicationDomain(ICommunicationRepository communicationRepository)
        {
            this.communicationRepository = communicationRepository;
        }
        public async Task<List<CommunicationDTO>> GetCommunicationInboxAsync( long userId)
        {
            return await communicationRepository.GetCommunicationInboxAsync(userId);
        }
        public async Task<List<CommunicationDTO>> GetCommunicationOutboxAsync(long userId)
        {
            return await communicationRepository.GetCommunicationOutboxAsync(userId);
        }

        public async Task<List<CommunicationDTO>> GetCommunicationDetailAsync(long id, int type, long userId)
        {
            return await communicationRepository.GetCommunicationDetailAsync(id,type,userId);
        }

        public async Task<List<CommunicationTemplateDTO>> GetCommunicationTemplateAsync(bool isActiveOnly,long userId)
        {
            return await communicationRepository.GetCommunicationTemplateAsync(isActiveOnly,userId);
        }

        public async Task<bool> SaveComposeCommunicationAsync(string subject, string messageText, List<UserContactDTO> toUsers, bool isGroup, DateTime? date, long? templateId,int communicationType, UserContactDTO fromUser, long userId)
        {
            return await communicationRepository.SaveComposeCommunicationAsync(subject, messageText, toUsers, isGroup, date, templateId, communicationType,fromUser, userId);
        }
        public async Task<bool> SaveReplyCommunicationAsync(long id, string subject, string messageText,long toUserId, UserContactDTO toUser, UserContactDTO fromUser, long userId)
        {
            return await communicationRepository.SaveReplyCommunicationAsync(id,subject, messageText, toUserId,toUser,fromUser, userId);
        }

        public async Task<UserContactDTO> GetFromUserCommunicationAsync(long userId)
        {
            return  await communicationRepository.GetFromUserCommunicationAsync(userId);
        }

        public async Task<List<UserContactDTO>> GetToUserCommunicationAsync(List<ReferenceDTO> toGroup, long userId)
        {
            return await communicationRepository.GetToUserCommunicationAsync(toGroup, userId);
        }

        public async Task<List<CommunicationDTO>> GetSMSCommunicationOutboxAsync(long userId)
        {
            return await communicationRepository.GetSMSCommunicationOutboxAsync(userId);
        }

        public async Task<bool> SaveCommunicationTemplate(long id, string name, string content, bool isSenderTemplate, bool isActive, long userId)
        {
            return await communicationRepository.SaveCommunicationTemplate(id,name,content,isSenderTemplate,isActive, userId);
        }
    }
}
