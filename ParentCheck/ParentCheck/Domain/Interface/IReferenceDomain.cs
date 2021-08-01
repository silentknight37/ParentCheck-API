using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public interface IReferenceDomain
    {
        Task<List<ReferenceDTO>> GetReferenceByTypeAsync(int referenceTypeId, long userId);
        Task<List<UserContactDTO>> GetUserContactAsync(string name, long userId);
        Task<List<UserContactDTO>> GetAllUserContactAsync(int sendType, long userId);
    }
}
