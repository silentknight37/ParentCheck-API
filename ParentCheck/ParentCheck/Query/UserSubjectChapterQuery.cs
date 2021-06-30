using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserSubjectChapterQuery : IRequest<UserSubjectChapterEnvelop>
    {
        public UserSubjectChapterQuery(long instituteClassSubjectId, long userId)
        {
            this.UserId = userId;
            this.InstituteClassSubjectId = instituteClassSubjectId;
        }

        public long InstituteClassSubjectId { get; set; }
        public long UserId { get; set; }
    }
}
