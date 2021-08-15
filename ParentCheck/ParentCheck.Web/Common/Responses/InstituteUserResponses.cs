using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class InstituteUserResponses
    {
        public List<InstituteUser> instituteUsers { get; set; }

        public static InstituteUserResponses PopulateInstituteUserResponses(List<InstituteUserDTO> instituteUserDTOs)
        {
            var instituteUserResponses = new InstituteUserResponses();

            instituteUserResponses.instituteUsers = new List<InstituteUser>();

            foreach (var instituteUserDTO in instituteUserDTOs)
            {
                var users = new InstituteUser
                {
                    id = instituteUserDTO.UserId,
                    firstName = instituteUserDTO.FirstName,
                    lastName = instituteUserDTO.LastName,
                    role = instituteUserDTO.Role,
                    dateOfBirth = instituteUserDTO.DateOfBirth,
                    userName = instituteUserDTO.UserName
                };

                instituteUserResponses.instituteUsers.Add(users);
            }

            return instituteUserResponses;
        }
    }

    public class InstituteUser
    {
        public long id { get; set; }
        public string role { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public DateTime dateOfBirth { get; set; }
    }
}
