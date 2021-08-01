using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class UserCommunicationEnvelop
    {
        public UserCommunicationEnvelop(List<CommunicationDTO> communicationDTOs)
        {
            this.Communications = communicationDTOs;
        }

        public List<CommunicationDTO>  Communications { get; set; }
    }
}
