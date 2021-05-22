using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class RoleQuery:IRequest<RoleEnvelop>
    {
        public RoleQuery()
        {
             
        }
    }
}
