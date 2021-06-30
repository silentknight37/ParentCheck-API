using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class UserClassDTO
    {
        public UserClassDTO()
        {
            Subjects = new List<UserSubjectDTO>();
        }

        public string UserClass { get; set; }
        public List<UserSubjectDTO> Subjects { get; set; }
    }
}
