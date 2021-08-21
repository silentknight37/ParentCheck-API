using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class InstituteUserDTO
    {
        public long UserId { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Admission { get; set; }
        public string Mobile { get; set; }
        public long ParentId { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public string ParentUsername { get; set; }
        public DateTime ParentDateOfBirth { get; set; }
        public string ParentMobile { get; set; }
        public bool IsActive { get; set; }
    }
}
