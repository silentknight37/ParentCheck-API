using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public class UserDomain: IUserDomain
    {
        private readonly IUserRepository _userRepository;

        public UserDomain(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDTO> GetUserAsync(long? userId, long? instituteId, string username, string admission)
        {
            return await _userRepository.GetUserAsync( userId,instituteId,username, admission);
        }
        public async Task<UserDTO> GetUserAuthenticateAsync(string username, string password)
        {
            return await _userRepository.GetUserAuthenticateAsync(username, password);
        }

    }
}
