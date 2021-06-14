using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserTopicContentsQuery : IRequest<UserTopicContentEnvelop>
    {
        public UserTopicContentsQuery(long instituteChapterTopicId, long userId)
        {
            this.UserId = userId;
            this.InstituteChapterTopicId = instituteChapterTopicId;
        }

        public long InstituteChapterTopicId { get; set; }
        public long UserId { get; set; }
    }
}
