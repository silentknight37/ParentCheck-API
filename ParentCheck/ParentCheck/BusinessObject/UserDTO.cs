using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class UserDTO
    {
        public long UserId { get; set; }
        public long InstituteId { get; set; }
        public long RoleId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string EncryptedFileName { get; set; }
        public string DateOfBirth { get; set; }
        public string StudentName { get; set; }
        public string FileName { get; set; }
        public string Admission { get; set; }
        public bool IsValidUser { get; set; }
    }
}
