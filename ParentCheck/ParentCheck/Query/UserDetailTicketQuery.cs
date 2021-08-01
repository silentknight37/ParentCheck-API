using MediatR;
using ParentCheck.Common;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserDetailTicketQuery : IRequest<DetailTicketEnvelop>
    {
        public UserDetailTicketQuery(long id, long userId)
        {
            this.Id = id;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public long UserId { get; set; }
    }
}
