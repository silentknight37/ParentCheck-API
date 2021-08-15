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
    public class TopicContentQueryHandler : IRequestHandler<TopicContentQuery, AcademicTopicContentEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public TopicContentQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<AcademicTopicContentEnvelop> Handle(TopicContentQuery topicContentQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var topicContents = await settingDomain.GetTopicContent(topicContentQuery.TopicId,topicContentQuery.UserId);
            return new AcademicTopicContentEnvelop(topicContents);
        }
    }
}
