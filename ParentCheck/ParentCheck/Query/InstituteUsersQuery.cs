using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class InstituteUsersQuery : IRequest<InstituteUsersEnvelop>
    {
        public InstituteUsersQuery(string searchValue, int? roleId,long userId)
        {
            this.SearchValue = searchValue;
            this.RoleId = roleId;
            this.UserId = userId;
        }

        public string SearchValue { get; set; }
        public int? RoleId { get; set; }
        public long UserId { get; set; }
    }
}
