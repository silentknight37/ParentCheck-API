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
    public class ChapterTopicQueryHandler : IRequestHandler<ChapterTopicQuery, AcademicChapterTopicEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public ChapterTopicQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<AcademicChapterTopicEnvelop> Handle(ChapterTopicQuery chapterTopicQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var chapterTopics = await settingDomain.GetChapterTopic(chapterTopicQuery.ChapterId,chapterTopicQuery.UserId);
            return new AcademicChapterTopicEnvelop(chapterTopics);
        }
    }
}
