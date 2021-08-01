using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class UserCommunicationDetailEnvelop
    {
        public UserCommunicationDetailEnvelop(List<CommunicationDTO> communicationDTOs)
        {
            this.Communications = communicationDTOs;
        }

        public List<CommunicationDTO> Communications { get; set; }
    }
}
