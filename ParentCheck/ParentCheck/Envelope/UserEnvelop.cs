using ParentCheck.BusinessObject;

namespace ParentCheck.Envelope
{
    public class UserEnvelop
    {
        public UserEnvelop(UserDTO user)
        {
            this.User = user;
        }

        public UserDTO User { get; set; }
    }
}
