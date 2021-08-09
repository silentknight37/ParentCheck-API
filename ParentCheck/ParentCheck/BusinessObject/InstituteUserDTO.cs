using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class InstituteUserDTO
    {
        public long UserId { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string HeadTeacherUser { get; set; }
        public string ClassTeacherUser { get; set; }
        public string ParentUser { get; set; }
        public string StudentUser { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
