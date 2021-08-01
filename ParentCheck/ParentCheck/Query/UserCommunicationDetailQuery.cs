using MediatR;
using ParentCheck.Common;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserCommunicationDetailQuery : IRequest<UserCommunicationDetailEnvelop>
    {
        public UserCommunicationDetailQuery(long id, int type, long userId)
        {
            this.Id = id;
            this.Type = type;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public int Type { get; set; }
        public long UserId { get; set; }
    }
}
