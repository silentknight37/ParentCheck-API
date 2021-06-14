using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserSubjectQuery : IRequest<UserSubjectEnvelop>
    {
        public UserSubjectQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
