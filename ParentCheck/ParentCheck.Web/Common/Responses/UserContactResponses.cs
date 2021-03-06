using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class UserContactResponses
    {
        public List<UserContact> userContacts { get; set; }

        public static UserContactResponses PopulateUserContactsResponses(List<UserContactDTO> userContactDTOs)
        {
            var userContactResponses = new UserContactResponses();
            userContactResponses.userContacts = new List<UserContact>();

            foreach (var userContact in userContactDTOs)
            {
                var uContact = new UserContact
                {
                    id= userContact.UserId,
                    fullName= userContact.UserFullName,
                    email= userContact.Email,
                    mobile= userContact.Mobile
                };

                userContactResponses.userContacts.Add(uContact);
            }

            return userContactResponses;
        }
    }

    public class UserContact
    {
        public long id { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
    }
}
