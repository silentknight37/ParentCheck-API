using MediatR;
using ParentCheck.Common;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserCommunicationInboxQuery : IRequest<UserCommunicationEnvelop>
    {
        public UserCommunicationInboxQuery( long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
