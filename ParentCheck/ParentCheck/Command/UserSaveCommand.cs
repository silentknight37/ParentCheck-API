using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class UserSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public UserSaveCommand(long id,string firstName, string lastName,string password, int roleId,long? parentUserid, string username,DateTime dateOfBirth, bool isActive, long userId)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Password = password;
            this.RoleId = roleId;
            this.ParentUserid = parentUserid;
            this.Username = username;
            this.DateOfBirth = dateOfBirth;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public long? ParentUserid { get; set; }
        public string Username { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
