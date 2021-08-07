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
                token= token
            };

            return userResponses;
        }
    }

    public class User
    {
        public long id { get; set; }
        public long instituteId { get; set; }
        public string fullName { get; set; }
        public string token { get; set; }
        public long roleId { get; set; }
    }
}
