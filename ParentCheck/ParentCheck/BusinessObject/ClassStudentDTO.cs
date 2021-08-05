using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class ClassStudentDTO
    {
        public long Id { get; set; }
        public long InstituteUserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
    }
}
