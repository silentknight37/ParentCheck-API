using MediatR;
using ParentCheck.Common;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserSmsCommunicationOutboxQuery : IRequest<UserCommunicationEnvelop>
    {
        public UserSmsCommunicationOutboxQuery( long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
