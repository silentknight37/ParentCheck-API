using MediatR;
using ParentCheck.BusinessObject;
using ParentCheck.Envelope;
using System;
using System.Collections.Generic;

namespace ParentCheck.Query
{
    public class ComposeCommunicationCommand : IRequest<RequestSaveEnvelop>
    {
        public ComposeCommunicationCommand(string subject, string messageText, List<UserContactDTO> toUsers,List<ReferenceDTO> toGroups, bool isGroup, DateTime? fromDate, DateTime? toDate, long? templateId,int communicationType, long userId)
        {
            this.Subject = subject;
            this.MessageText = messageText;
            this.ToUsers = toUsers;
            this.ToGroups = toGroups;
            this.IsGroup = isGroup;
            this.FromDate = fromDate;
            this.ToDate = toDate;
            this.TemplateId = templateId;
            this.CommunicationType = communicationType;
            this.UserId = userId;
        }

        public string Subject { get; set; }
        public string MessageText { get; set; }
        public List<UserContactDTO> ToUsers { get; set; }
        public List<ReferenceDTO> ToGroups { get; set; }
        public bool IsGroup { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public long? TemplateId { get; set; }
        public int CommunicationType { get; set; }
        public long UserId { get; set; }
    }
}
