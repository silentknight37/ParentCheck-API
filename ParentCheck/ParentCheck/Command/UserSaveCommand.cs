using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class UserSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public UserSaveCommand(long id,string firstName, string lastName,string password, string parentPassword, int roleId, string username,DateTime dateOfBirth,DateTime? parentDateOfBirth,
            string admission,string mobile,long parentId,string parentFirstName,string parentLastName,string parentUsername,string parentMobile, bool isActive, long userId)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Password = password;
            this.ParentPassword = parentPassword;
            this.RoleId = roleId;
            this.Admission = admission;
            this.Mobile = mobile;
            this.ParentId = parentId;
            this.ParentFirstName = parentFirstName;
            this.ParentLastName = parentLastName;
            this.ParentUsername = parentUsername;
            this.ParentMobile = parentMobile;
            this.Username = username;
            this.DateOfBirth = dateOfBirth;
            this.ParentDateOfBirth = parentDateOfBirth;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ParentPassword { get; set; }
        public int RoleId { get; set; }
        public string Username { get; set; }
        public string Admission { get; set; }
        public string Mobile { get; set; }
        public long ParentId { get; set; }
        public string ParentFirstName { get; set; }
        public string ParentLastName { get; set; }
        public string ParentUsername { get; set; }
        public string ParentMobile { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? ParentDateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
