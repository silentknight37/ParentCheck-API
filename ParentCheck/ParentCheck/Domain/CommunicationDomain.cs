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

        public async Task<CommunicationDTO> GetCommunicationDetailAsync(long id, int type, long userId)
        {
            return await communicationRepository.GetCommunicationDetailAsync(id,type,userId);
        }
    }
}
