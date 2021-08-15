using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class UserSaveRequest
    {
        public long id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public int roleId { get; set; }
        public long? parentUserid { get; set; }
        public string username { get; set; }
        public DateTime dateOfBirth { get; set; }
        public bool isActive { get; set; }
    }
}
