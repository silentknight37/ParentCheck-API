using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserStudentsContactQuery : IRequest<UserContactEnvelop>
    {
        public UserStudentsContactQuery(int sendType, long userId)
        {
            this.SendType = sendType;
            this.UserId = userId;
        }

        public int SendType { get; set; }
        public long UserId { get; set; }
    }
}
