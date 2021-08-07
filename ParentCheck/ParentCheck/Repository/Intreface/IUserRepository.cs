using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository.Intreface
{
    public interface IUserRepository
    {
        Task<UserDTO> GetUserAsync(long userId);
        Task<UserDTO> GetUserAuthenticateAsync(string username, string password);
    }
}
