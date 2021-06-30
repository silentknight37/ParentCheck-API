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

        public RequestSaveEnvelop(bool created, string successMessage,long id, Error error)
        {
            this.Created = created;
            this.SuccessMessage = successMessage;
            this.Error = error;
            this.Id = id;
        }

        public bool Created { get; set; }
        public long Id { get; set; }
        public string SuccessMessage { get; set; }
        public Error Error { get; set; }
    }
}
