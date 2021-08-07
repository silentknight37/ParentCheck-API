using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public interface IUserDomain
    {
        Task<UserDTO> GetUserAsync(long userId);
        Task<UserDTO> GetUserAuthenticateAsync(string username, string password);
    }
}
