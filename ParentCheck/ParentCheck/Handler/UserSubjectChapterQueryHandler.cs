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
    public class UserSubjectChapterQueryHandler : IRequestHandler<UserSubjectChapterQuery, UserSubjectChapterEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public UserSubjectChapterQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<UserSubjectChapterEnvelop> Handle(UserSubjectChapterQuery subjectChapterQuery,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            var userSubjects = await classRoomDomain.GetUserSubjectChaptersAsync(subjectChapterQuery.InstituteClassSubjectId, subjectChapterQuery.UserId);

            return new UserSubjectChapterEnvelop(userSubjects);
        }
    }
}
