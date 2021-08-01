using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class UserContactDTO
    {
        public long UserId { get; set; }
        public string UserFullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
