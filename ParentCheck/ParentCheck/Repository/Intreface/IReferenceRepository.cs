using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository.Intreface
{
    public interface IReferenceRepository
    {
        Task<List<ReferenceDTO>> GetReferenceByTypeAsync(int referenceTypeId, long userId);
        Task<List<UserContactDTO>> GetUserContactAsync(string name, long userId);
    }
}
