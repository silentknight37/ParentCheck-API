using MediatR;
using ParentCheck.Common;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserCommunicationOutboxQuery : IRequest<UserCommunicationEnvelop>
    {
        public UserCommunicationOutboxQuery( long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
