using MediatR;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Factory;
using ParentCheck.Factory.Intreface;
using ParentCheck.Query;
using System.Threading;
using System.Threading.Tasks;

namespace ParentCheck.Handler
{
    public class UserChapterTopicQueryHandler : IRequestHandler<UserChapterTopicQuery, UserChapterTopicsEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public UserChapterTopicQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<UserChapterTopicsEnvelop> Handle(UserChapterTopicQuery userChapterTopicQuery,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            var userChapterTopics = await classRoomDomain.GetUserChaptersTopicsAsync(userChapterTopicQuery.InstituteSubjectChapterId, userChapterTopicQuery.UserId);

            return new UserChapterTopicsEnvelop(userChapterTopics);
        }
    }
}
