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
    public class LibraryQueryHandler : IRequestHandler<LibraryQuery, LibraryEnvelop>
    {
        private readonly IClassRoomFactory classRoomFactory;

        public LibraryQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.classRoomFactory = new ClassRoomFactory(parentcheckContext);
        }

        public async Task<LibraryEnvelop> Handle(LibraryQuery libraryQuery,CancellationToken cancellationToken)
        {
            var classRoomDomain = this.classRoomFactory.Create();
            var library = await classRoomDomain.GetLibraryAsync(libraryQuery.UserId);

            return new LibraryEnvelop(library);
        }
    }
}
