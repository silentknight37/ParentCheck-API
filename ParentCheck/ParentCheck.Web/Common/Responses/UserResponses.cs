using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class UserResponses
    {
        public User user { get; set; }

        public static UserResponses PopulateUserResponses(string token,UserDTO userDTO)
        {
            var userResponses = new UserResponses();
            userResponses.user = new User
            {
                id = userDTO.UserId,
                fullName = $"{userDTO.FirstName} {userDTO.LastName}",
                roleId = userDTO.RoleId,
                instituteId = userDTO.InstituteId,
                token= token,
                email= userDTO.UserName,
                image= userDTO.ImageUrl,
                isValid = userDTO.IsValidUser
            };

            return userResponses;
        }
    }

    public class User
    {
        public long id { get; set; }
        public long instituteId { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string image { get; set; }
        public string token { get; set; }
        public long roleId { get; set; }
        public bool isValid { get; set; }
    }
}
