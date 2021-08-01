using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserAllContactQuery : IRequest<UserContactEnvelop>
    {
        public UserAllContactQuery(int sendType, long userId)
        {
            this.SendType = sendType;
            this.UserId = userId;
        }

        public int SendType { get; set; }
        public long UserId { get; set; }
    }
}
