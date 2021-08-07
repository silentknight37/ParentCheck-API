using Microsoft.EntityFrameworkCore;
using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository
{
    public class UserRepository: IUserRepository
    {
        private ParentCheckContext _parentcheckContext;

        public UserRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }

        public async Task<UserDTO> GetUserAsync(long userId)
        {
            UserDTO returnUser = new UserDTO();
            var user = await (from u in _parentcheckContext.User
                              join iu in _parentcheckContext.InstituteUser on u.Id equals iu.UserId
                              where iu.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  iu.Id,
                                  iu.InstituteId,
                                  iu
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                returnUser.FirstName = user.FirstName;
                returnUser.LastName = user.LastName;
                returnUser.UserId = user.Id;
                returnUser.InstituteId = user.InstituteId;
                return returnUser;
            }

            return null;
        }

        public async Task<UserDTO> GetUserAuthenticateAsync(string username, string password)
        {
            UserDTO returnUser = new UserDTO();
            var user = await (from u in _parentcheckContext.User
                              join iu in _parentcheckContext.InstituteUser on u.Id equals iu.UserId
                              //where iu.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  iu.Id,
                                  iu.InstituteId,
                                  iu
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                returnUser.FirstName = user.FirstName;
                returnUser.LastName = user.LastName;
                returnUser.UserId = user.Id;
                returnUser.InstituteId = user.InstituteId;
                return returnUser;
            }

            return null;
        }
    }
}
