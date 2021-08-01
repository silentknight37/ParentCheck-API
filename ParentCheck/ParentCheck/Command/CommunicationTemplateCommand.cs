using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class CommunicationTemplateCommand : IRequest<RequestSaveEnvelop>
    {
        public CommunicationTemplateCommand(long id,string name,string content,bool isSenderTemplate,bool isActive, long userId)
        {
            this.Id = id;
            this.Name = name;
            this.Content = content;
            this.IsSenderTemplate = isSenderTemplate;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public bool IsSenderTemplate { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
