using ParentCheck.BusinessObject;
using ParentCheck.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository.Intreface
{
    public interface ICommunicationRepository
    {
        Task<List<CommunicationDTO>> GetCommunicationInboxAsync(long userId);
        Task<List<CommunicationDTO>> GetCommunicationOutboxAsync(long userId);
        Task<CommunicationDTO> GetCommunicationDetailAsync(long id, int type, long userId);
    }
}
