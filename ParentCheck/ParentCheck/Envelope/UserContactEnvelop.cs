using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class UserContactEnvelop
    {
        public UserContactEnvelop(List<UserContactDTO> userContactDTOs)
        {
            this.UserContacts = userContactDTOs;
        }

        public List<UserContactDTO> UserContacts { get; set; }
    }
}
