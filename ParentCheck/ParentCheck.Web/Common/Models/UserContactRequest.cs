using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class UserContactRequest
    {
        public long Id { get; set; }
        public string ToValue { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
    }
}
