using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class SubjectChapterSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public SubjectChapterSaveCommand(long id,long subjectId, string chapter, bool isActive, long userId)
        {
            this.Id = id;
            this.SubjectId = subjectId;
            this.Chapter = chapter;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public long SubjectId { get; set; }
        public string Chapter { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
