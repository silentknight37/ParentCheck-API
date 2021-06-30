using ParentCheck.Data;
using ParentCheck.Domain;
using ParentCheck.Factory.Intreface;
using ParentCheck.Repository;

namespace ParentCheck.Factory
{
    public class ClassRoomFactory : IClassRoomFactory
    {
        private ParentCheckContext _parentCheckContext;

        public ClassRoomFactory(ParentCheckContext parentCheckContext)
        {
            _parentCheckContext = parentCheckContext;
        }

        IClassRoomDomain IClassRoomFactory.Create()
        {
            return new ClassRoomDomain(new ClassRoomRepository(_parentCheckContext));
        }
    }
}
