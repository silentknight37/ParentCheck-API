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
    public class UserSubjectQueryHandler : IRequestHandler<UserSubjectQuery, UserSubjectEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public UserSubjectQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<UserSubjectEnvelop> Handle(UserSubjectQuery calenderEvent,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            var userSubjects = await classRoomDomain.GetUserSubjectAsync(calenderEvent.UserId);

            return new UserSubjectEnvelop(userSubjects);
        }
    }
}
