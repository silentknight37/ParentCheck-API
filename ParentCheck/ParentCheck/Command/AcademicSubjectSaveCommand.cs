using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class AcademicSubjectSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public AcademicSubjectSaveCommand(long id,string subject, string descriptionText, bool isActive, long userId)
        {
            this.Id = id;
            this.Subject = subject;
            this.DescriptionText = descriptionText;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public string Subject { get; set; }
        public string DescriptionText { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
