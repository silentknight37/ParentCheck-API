using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class InstituteUsersQuery : IRequest<InstituteUsersEnvelop>
    {
        public InstituteUsersQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
