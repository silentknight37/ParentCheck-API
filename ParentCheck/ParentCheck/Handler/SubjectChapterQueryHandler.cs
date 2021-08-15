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
    public class SubjectChapterQueryHandler : IRequestHandler<SubjectChapterQuery, AcademicSubjectChapterEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public SubjectChapterQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<AcademicSubjectChapterEnvelop> Handle(SubjectChapterQuery subjectChapterQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var academicClasses = await settingDomain.GetSubjectChapter(subjectChapterQuery.SubjectId,subjectChapterQuery.UserId);
            return new AcademicSubjectChapterEnvelop(academicClasses);
        }
    }
}
