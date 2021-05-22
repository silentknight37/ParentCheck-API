using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserQuery:IRequest<UserEnvelop>
    {
        public UserQuery()
        {
             
        }
    }
}
