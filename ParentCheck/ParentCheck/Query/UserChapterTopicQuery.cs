using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserChapterTopicQuery : IRequest<UserChapterTopicsEnvelop>
    {
        public UserChapterTopicQuery(long instituteSubjectChapterId, long userId)
        {
            this.UserId = userId;
            this.InstituteSubjectChapterId = instituteSubjectChapterId;
        }

        public long InstituteSubjectChapterId { get; set; }
        public long UserId { get; set; }
    }
}
