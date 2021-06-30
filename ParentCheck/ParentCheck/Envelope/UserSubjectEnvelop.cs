using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class UserSubjectEnvelop
    {
        public UserSubjectEnvelop(UserClassDTO userClass)
        {
            this.UserClass = userClass;
        }

        public UserClassDTO UserClass { get; set; }
    }
}
