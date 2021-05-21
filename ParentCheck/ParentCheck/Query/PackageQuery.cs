using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class PackageQuery:IRequest<PackageEnvelop>
    {
        public PackageQuery()
        {
             
        }
    }
}
