using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class InstituteUsersQuery : IRequest<InstituteUsersEnvelop>
    {
        public InstituteUsersQuery(string searchValue,long userId)
        {
            this.SearchValue = searchValue;
            this.UserId = userId;
        }

        public string SearchValue { get; set; }
        public long UserId { get; set; }
    }
}
