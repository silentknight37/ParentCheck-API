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
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u.ImageUrl,
                                  u.FileName
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                returnUser.FirstName = user.FirstName;
                returnUser.LastName = user.LastName;
                returnUser.UserId = user.Id;
                returnUser.InstituteId = user.InstituteId;
                returnUser.ImageUrl = user.ImageUrl;
                returnUser.FileName = user.FileName;
                return returnUser;
            }

            return null;
        }

        public async Task<UserDTO> GetUserAuthenticateAsync(string username, string password)
        {
            UserDTO returnUser = new UserDTO();
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Username==username && u.Password==password
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u.RoleId,
                                  u.Username,
                                  u.ImageUrl
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                returnUser.FirstName = user.FirstName;
                returnUser.LastName = user.LastName;
                returnUser.UserId = user.Id;
                returnUser.InstituteId = user.InstituteId;
                returnUser.RoleId = user.RoleId;
                returnUser.UserName = user.Username;
                returnUser.ImageUrl = user.ImageUrl;
                returnUser.IsValidUser = true;
                return returnUser;
            }

            return null;
        }
    }
}
