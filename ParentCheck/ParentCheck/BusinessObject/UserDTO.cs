using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class UserDTO
    {
        public long UserId { get; set; }
        public long InstituteId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
