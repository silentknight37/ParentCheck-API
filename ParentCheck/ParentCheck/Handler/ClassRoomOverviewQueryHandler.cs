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
    public class ClassRoomOverviewQueryHandler : IRequestHandler<ClassRoomOverviewQuery, ClassRoomOverviewEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public ClassRoomOverviewQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<ClassRoomOverviewEnvelop> Handle(ClassRoomOverviewQuery classRoomOverviewQuery,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            var classRoomOverview = await classRoomDomain.GetClassRoomOverviewAsync(classRoomOverviewQuery.IsToday, classRoomOverviewQuery.IsThisWeek, classRoomOverviewQuery.IsNextWeek, classRoomOverviewQuery.IsCustom,classRoomOverviewQuery.FromDate, classRoomOverviewQuery.ToDate, classRoomOverviewQuery.SubjectId, classRoomOverviewQuery.InstituteTermsId, classRoomOverviewQuery.UserId);

            return new ClassRoomOverviewEnvelop(classRoomOverview);
        }
    }
}
