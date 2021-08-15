using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class AcademicClassSubjectSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public AcademicClassSubjectSaveCommand(long id,long classId, long subjectId, long responsibleUserId, bool isActive, long userId)
        {
            this.Id = id;
            this.ClassId = classId;
            this.SubjectId = subjectId;
            this.ResponsibleUserId = responsibleUserId;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public long ClassId { get; set; }
        public long SubjectId { get; set; }
        public long ResponsibleUserId { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
