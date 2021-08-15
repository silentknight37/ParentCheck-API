using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class SubjectChapterQuery : IRequest<AcademicSubjectChapterEnvelop>
    {
        public SubjectChapterQuery(long subjectId, long userId)
        {
            this.SubjectId = subjectId;
            this.UserId = userId;
        }

        public long SubjectId { get; set; }
        public long UserId { get; set; }
    }
}
