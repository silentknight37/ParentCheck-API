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
    public class SubjectQueryHandler : IRequestHandler<SubjectQuery, SubjectEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public SubjectQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<SubjectEnvelop> Handle(SubjectQuery subjectQuery,CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            var subjects = await settingDomain.GetSubject(subjectQuery.UserId);
            return new SubjectEnvelop(subjects);
        }
    }
}
