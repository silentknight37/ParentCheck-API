using MailKit.Net.Smtp;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Factory;
using ParentCheck.Factory.Intreface;
using ParentCheck.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ParentCheck.Handler
{
    public class UserSaveCommandHandler : IRequestHandler<UserSaveCommand, RequestSaveEnvelop>
    {
        private readonly ISettingFactory settingFactory;
        private readonly IMediator mediator;
        public UserSaveCommandHandler(ParentCheckContext parentcheckContext, IMediator mediator)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
            this.mediator = mediator;
        }

        public async Task<RequestSaveEnvelop> Handle(UserSaveCommand userSaveCommand, CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            try
            {
                    var userUserNameValidate = await mediator.Send((IRequest<UserEnvelop>)new UserQuery(null, null, userSaveCommand.Username,string.Empty));
                    if (userUserNameValidate.User != null && userUserNameValidate.User.UserId!= userSaveCommand.Id)
                    {
                        var errorMessage = "Request fail due to username already exists";
                        Error error = new Error(ErrorType.FORBIDDEN, errorMessage);
                        return new RequestSaveEnvelop(false, string.Empty, error);
                    }

                    var userAdmissionValidate = await mediator.Send((IRequest<UserEnvelop>)new UserQuery(null, null, string.Empty, userSaveCommand.Admission));
                    if (userAdmissionValidate.User != null && userAdmissionValidate.User.UserId != userSaveCommand.Id)
                    {
                        var errorMessage = "Request fail due to index/admission already exists";
                        Error error = new Error(ErrorType.FORBIDDEN, errorMessage);
                        return new RequestSaveEnvelop(false, string.Empty, error);
                    }

                var password = "Welcome123";//userSaveCommand.Password;
                var parentPassword = "Welcome123"; //userSaveCommand.ParentPassword;
                if (userSaveCommand.Id == 0)
                {
                    password = GenerateRandomPassword();
                    parentPassword = GenerateRandomPassword();
                }

                if (userSaveCommand.ParentId == 0)
                {
                    parentPassword = GenerateRandomPassword();
                }
                var response = await settingDomain.SaveInstituteUser(
                    userSaveCommand.Id, 
                    userSaveCommand.FirstName, 
                    userSaveCommand.LastName, 
                    userSaveCommand.RoleId, 
                    userSaveCommand.Username,
                    userSaveCommand.DateOfBirth, 
                    userSaveCommand.Mobile,
                    userSaveCommand.Admission,
                    password,
                    userSaveCommand.ParentId,
                    userSaveCommand.ParentFirstName,
                    userSaveCommand.ParentLastName,
                    userSaveCommand.ParentUsername,
                    userSaveCommand.ParentDateOfBirth,
                    userSaveCommand.ParentMobile,
                    parentPassword,
                    userSaveCommand.IsActive, 
                    userSaveCommand.UserId);

                if (!response)
                {
                    var errorMessage = "Request fail due to invalid user";
                    Error error = new Error(ErrorType.UNAUTHORIZED, errorMessage);
                    return new RequestSaveEnvelop(false, string.Empty, error);
                }
                if (userSaveCommand.Id == 0)
                {
                    await SendEmail($"{userSaveCommand.FirstName} {userSaveCommand.LastName}", userSaveCommand.Username, password);
                    if (userSaveCommand.RoleId == (int)EnumRole.Student)
                    {
                        await SendEmail($"{userSaveCommand.ParentFirstName} {userSaveCommand.ParentLastName}", userSaveCommand.ParentUsername, parentPassword);
                    }
                }
                return new RequestSaveEnvelop(response, "Request process successfully", null);
            }
            catch (System.Exception e)
            {
                var errorMessage = e.Message;
                Error error = new Error(ErrorType.BAD_REQUEST, errorMessage);
                return new RequestSaveEnvelop(false, string.Empty, error);
            }
        }
        private async Task<bool> SendEmail(string name,string email,string password)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Parent Check", "connect@parentcheck.lk"));
            message.To.Add(new MailboxAddress(name, email));
            message.Subject = "Login Details For Parent Check";

            var messageText = $"<html><body><div style = 'padding: 50px;background-image: url(https://parentcheck.lk/assets/images/logo-color.png); background-position: center; background-repeat: no-repeat; background-size: contain;'></div ><div style = 'background-color: #fff;padding: 10px;'><h2 style = 'text-align: center;'> Welcome to Parent Check</h2><p> Hi {name} </p><p> You can login to your account using the following details. </p><p><b> Username: {email} </b></p><p><b> Password: {password}</b></p><p> Thank you </p><p> Parent Check </p></div></body></html> ";

            message.Body = new TextPart("html")
            {
                Text = messageText
            };
            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect("mail.parentcheck.lk", 8889,false);
                    client.Authenticate("connect@parentcheck.lk", "x3wSh@uB");
                    client.Send(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (System.Exception e)
            {
                return false;
            }
        }
        private string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }
    }
}
