using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class UserSubmitedAssignmentFileEnvelop
    {
        public UserSubmitedAssignmentFileEnvelop(UserSubmitedAssignmentFileDTO submitedAssignmentFile)
        {
            this.SubmitedAssignmentFile = submitedAssignmentFile;
        }

        public UserSubmitedAssignmentFileDTO SubmitedAssignmentFile { get; set; }
    }
}
