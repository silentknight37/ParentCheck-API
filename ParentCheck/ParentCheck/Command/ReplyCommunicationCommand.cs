using MediatR;
using ParentCheck.BusinessObject;
using ParentCheck.Envelope;
using System;
using System.Collections.Generic;

namespace ParentCheck.Query
{
    public class ReplyCommunicationCommand : IRequest<RequestSaveEnvelop>
    {
        public ReplyCommunicationCommand(string id,string subject, string messageText, long toUserId, long userId)
        {
            this.Id = id;
            this.Subject = subject;
            this.MessageText = messageText;
            this.ToUserId = toUserId;
            this.UserId = userId;
        }

        public string Id { get; set; }
        public string Subject { get; set; }
        public string MessageText { get; set; }
        public long ToUserId { get; set; }
        public long UserId { get; set; }
        public long GetId
        {
            get
            {
                return long.Parse(Id);
            }
        }
    }
}
