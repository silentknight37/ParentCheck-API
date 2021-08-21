using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserQuery:IRequest<UserEnvelop>
    {
        public UserQuery(long? userId, long? instituteId, string username, string admission)
        {
            this.UserId = userId;
            this.InstituteId = instituteId;
            this.Username = username;
            this.Admission = admission;
        }

        public long? UserId { get; set; }
        public long? InstituteId { get; set; }
        public string Username { get; set; }
        public string Admission { get; set; }
    }
}
