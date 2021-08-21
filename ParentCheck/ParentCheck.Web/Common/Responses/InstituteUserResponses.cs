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
                    roleId = instituteUserDTO.RoleId,
                    dateOfBirth = instituteUserDTO.DateOfBirth,
                    dateOfBirthFormated = instituteUserDTO.DateOfBirth.ToString("dd/MM/yyyy"),
                    userName = instituteUserDTO.UserName,
                    mobile = instituteUserDTO.Mobile,
                    admission = instituteUserDTO.Admission,
                    isActive = instituteUserDTO.IsActive,
                    parentId= instituteUserDTO.ParentId,
                    parentFirstName = instituteUserDTO.ParentFirstName,
                    parentLastName = instituteUserDTO.ParentLastName,
                    parentUsername = instituteUserDTO.ParentUsername,
                    parentMobile = instituteUserDTO.ParentMobile,
                    parentDateOfBirth = instituteUserDTO.ParentDateOfBirth,
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
        public int roleId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string admission { get; set; }
        public string mobile { get; set; }
        public bool isActive { get; set; }
        public string dateOfBirthFormated { get; set; }
        public DateTime dateOfBirth { get; set; }
        public long parentId { get; set; }
        public string parentFirstName { get; set; }
        public string parentLastName { get; set; }
        public string parentUsername { get; set; }
        public string parentMobile { get; set; }
        public DateTime parentDateOfBirth { get; set; }
    }
}
