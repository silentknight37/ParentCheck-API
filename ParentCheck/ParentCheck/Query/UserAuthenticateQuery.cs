using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class UserAuthenticateQuery : IRequest<UserEnvelop>
    {
        public UserAuthenticateQuery(string username,string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public string Username { get; set; }
        public string Password { get; set; }
    }
}
