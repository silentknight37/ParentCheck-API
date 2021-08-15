using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class InstituteUsersEnvelop
    {
        public InstituteUsersEnvelop(List<InstituteUserDTO> instituteUserDTOs)
        {
            this.InstituteUsers = instituteUserDTOs;
        }

        public List<InstituteUserDTO> InstituteUsers { get; set; }
    }
}
