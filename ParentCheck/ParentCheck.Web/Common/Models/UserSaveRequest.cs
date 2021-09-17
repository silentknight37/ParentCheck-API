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
        public string username { get; set; }
        public string admission { get; set; }
        public string mobile { get; set; }
        public string dateOfBirth { get; set; }
        public long parentId { get; set; }
        public string parentFirstName { get; set; }
        public string parentLastName { get; set; }
        public string parentUsername { get; set; }
        public string parentMobile { get; set; }
        public string parentDateOfBirth { get; set; }
        public bool isActive { get; set; }
    }
}
