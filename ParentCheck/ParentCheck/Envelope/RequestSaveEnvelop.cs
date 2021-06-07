using ParentCheck.BusinessObject;
using ParentCheck.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class RequestSaveEnvelop
    {
        public RequestSaveEnvelop(bool created,string successMessage, Error error)
        {
            this.Created = created;
            this.SuccessMessage = successMessage;
            this.Error = error;
        }

        public bool Created { get; set; }
        public string SuccessMessage { get; set; }
        public Error Error { get; set; }
    }
}
