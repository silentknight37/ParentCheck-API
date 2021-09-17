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

        public async Task<UserDTO> GetUserAsync(long? userId, long? instituteId, string username, string admission)
        {
            UserDTO returnUser = new UserDTO();
            var user = await (from u in _parentcheckContext.InstituteUser
                              where (userId== null || u.Id == userId) &&
                              (instituteId==null || u.InstituteId== instituteId) && 
                              (string.IsNullOrEmpty(username) || u.Username== username) &&
                              (string.IsNullOrEmpty(admission) || u.IndexNo== admission) 
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u.ImageUrl,
                                  u.FileName,
                                  u.DateOfBirth,
                                  u.EncryptedFileName,
                                  u.RoleId,
                                  u.IndexNo,
                                  u.Username
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                returnUser.UserName = user.Username;
                returnUser.FirstName = user.FirstName;
                returnUser.LastName = user.LastName;
                returnUser.UserId = user.Id;
                returnUser.InstituteId = user.InstituteId;
                returnUser.ImageUrl = user.ImageUrl;
                returnUser.FileName = user.FileName;
                returnUser.EncryptedFileName = user.EncryptedFileName;
                returnUser.DateOfBirth = user.DateOfBirth.ToString("yyyy MMMM dd");
                returnUser.Admission = user.IndexNo;

                if (user.RoleId == (int)EnumRole.Parent)
                {
                    var student = await _parentcheckContext.InstituteUser.FirstOrDefaultAsync(i => i.ParentUserid == user.Id);
                    if (student != null)
                    {
                        returnUser.StudentName = $"{student.IndexNo} - {student.FirstName} {student.LastName}";
                    }
                }
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
                                  u.ImageUrl,
                                  u.DateOfBirth
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
                returnUser.DateOfBirth = user.DateOfBirth.ToString("yyyy MMMM dd");

                if (user.RoleId == (int)EnumRole.Parent)
                {
                    var student = await _parentcheckContext.InstituteUser.FirstOrDefaultAsync(i => i.ParentUserid == user.Id);
                    if (student != null)
                    {
                        returnUser.StudentName = $"{student.IndexNo} - {student.FirstName} {student.LastName}";
                    }
                }

                return returnUser;
            }

            return null;
        }
    }
}
