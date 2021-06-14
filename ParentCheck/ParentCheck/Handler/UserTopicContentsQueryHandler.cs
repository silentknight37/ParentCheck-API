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
    public class UserTopicContentsQueryHandler : IRequestHandler<UserTopicContentsQuery, UserTopicContentEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public UserTopicContentsQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<UserTopicContentEnvelop> Handle(UserTopicContentsQuery topicContentsQuery,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            var userChapterTopics = await classRoomDomain.GetUserTopicContentAsync(topicContentsQuery.InstituteChapterTopicId, topicContentsQuery.UserId);

            return new UserTopicContentEnvelop(userChapterTopics);
        }
    }
}
