using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class UserCommunicationDetailEnvelop
    {
        public UserCommunicationDetailEnvelop(CommunicationDTO communicationDTO)
        {
            this.Communication = communicationDTO;
        }

        public CommunicationDTO Communication { get; set; }
    }
}
