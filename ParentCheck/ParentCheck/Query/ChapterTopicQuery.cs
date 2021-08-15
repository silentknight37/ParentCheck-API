using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class ChapterTopicQuery : IRequest<AcademicChapterTopicEnvelop>
    {
        public ChapterTopicQuery(long chapterId, long userId)
        {
            this.ChapterId = chapterId;
            this.UserId = userId;
        }

        public long ChapterId { get; set; }
        public long UserId { get; set; }
    }
}
