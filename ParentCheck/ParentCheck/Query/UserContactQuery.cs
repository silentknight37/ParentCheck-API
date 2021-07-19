using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserContactQuery : IRequest<UserContactEnvelop>
    {
        public UserContactQuery(string name, long userId)
        {
            this.Name = name;
            this.UserId = userId;
        }

        public string Name { get; set; }
        public long UserId { get; set; }
    }
}
