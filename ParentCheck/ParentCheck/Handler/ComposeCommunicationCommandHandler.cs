using MailKit.Net.Smtp;
using MediatR;
using MimeKit;
using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Factory;
using ParentCheck.Factory.Intreface;
using ParentCheck.Query;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ParentCheck.Handler
{
    public class ComposeCommunicationCommandHandler : IRequestHandler<ComposeCommunicationCommand, RequestSaveEnvelop>
    {
        private readonly ICommunicationFactory communicationFactory;
        private readonly IMediator mediator;

        public ComposeCommunicationCommandHandler(ParentCheckContext parentcheckContext, IMediator mediator)
        {
            this.communicationFactory = new CommunicationFactory(parentcheckContext);
            this.mediator = mediator;
        }

        public async Task<RequestSaveEnvelop> Handle(ComposeCommunicationCommand composeCommunicationCommand, CancellationToken cancellationToken)
        {
            var communicationDomain = this.communicationFactory.Create();
            try
            {
                var fromUser=await communicationDomain.GetFromUserCommunicationAsync( composeCommunicationCommand.UserId);

                if (fromUser==null)
                {
                    var errorMessage = "Request fail due to invalid user";
                    Error error = new Error(ErrorType.UNAUTHORIZED, errorMessage);
                    return new RequestSaveEnvelop(false, string.Empty, error);
                }
                var toUser = composeCommunicationCommand.ToUsers;
                if (composeCommunicationCommand.IsGroup)
                {
                    toUser= await communicationDomain.GetToUserCommunicationAsync(composeCommunicationCommand.ToGroups, composeCommunicationCommand.UserId);
                }
                switch (composeCommunicationCommand.CommunicationType)
                {
                    case (int)EnumCommunication.Email:
                        {
                            foreach (var user in toUser)
                            {
                                //var responceEmail=await SendEmail(composeCommunicationCommand.Subject, composeCommunicationCommand.MessageText, fromUser, user);

                                //if (!responceEmail)
                                //{
                                //    var errorMessage = "Email Sending Failed";
                                //    Error error = new Error(ErrorType.INTERNAL, errorMessage);
                                //    return new RequestSaveEnvelop(false, string.Empty, error);
                                //}
                            }
                        }
                        break;
                    case (int)EnumCommunication.SMS:
                        {
                            foreach (var user in toUser)
                            {
                                var responceSms = await SendSms(composeCommunicationCommand.Subject, composeCommunicationCommand.MessageText, fromUser, user);

                                if (!responceSms)
                                {
                                    var errorMessage = "Sms Sending Failed";
                                    Error error = new Error(ErrorType.INTERNAL, errorMessage);
                                    return new RequestSaveEnvelop(false, string.Empty, error);
                                }
                            }
                        }
                        break;
                    case (int)EnumCommunication.Event:
                        {
                            foreach (var user in toUser)
                            {
                                //var message = $"Event Date : {composeCommunicationCommand.FromDate.Value.ToShortDateString()} {composeCommunicationCommand.FromDate.Value.ToShortTimeString()} to {composeCommunicationCommand.ToDate.Value.ToShortDateString()} - {composeCommunicationCommand.ToDate.Value.ToShortTimeString()}</br>{composeCommunicationCommand.MessageText}";
                                //var responceEmail = await SendEmail(composeCommunicationCommand.Subject, message, fromUser, user);

                                //if (!responceEmail)
                                //{
                                //    var errorMessage = "Event Sending Failed";
                                //    Error error = new Error(ErrorType.INTERNAL, errorMessage);
                                //    return new RequestSaveEnvelop(false, string.Empty, error);
                                //}

                                var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new CalenderEventSaveCommand(composeCommunicationCommand.FromDate.Value, composeCommunicationCommand.ToDate.Value, composeCommunicationCommand.Subject, composeCommunicationCommand.MessageText, (int)EnumEventType.Event, user.UserId));

                            }
                        }
                        break;
                    case (int)EnumCommunication.Session:
                        {
                            foreach (var user in toUser)
                            {
                                //var message = $"Session Date : {composeCommunicationCommand.Date.Value.ToShortDateString()} - {composeCommunicationCommand.Date.Value.ToShortTimeString()}</br>{composeCommunicationCommand.MessageText}";
                                //var responceEmail = await SendEmail(composeCommunicationCommand.Subject, message, fromUser, user);

                                //if (!responceEmail)
                                //{
                                //    var errorMessage = "Session Sending Failed";
                                //    Error error = new Error(ErrorType.INTERNAL, errorMessage);
                                //    return new RequestSaveEnvelop(false, string.Empty, error);
                                //}

                                var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new CalenderEventSaveCommand(composeCommunicationCommand.FromDate.Value, composeCommunicationCommand.ToDate.Value, composeCommunicationCommand.Subject, composeCommunicationCommand.MessageText, (int)EnumEventType.Sessions, user.UserId));
                            }
                        }
                        break;
                    case (int)EnumCommunication.Template:
                        {
                            //template
                        }
                        break;
                }

                var response = await communicationDomain.SaveComposeCommunicationAsync(composeCommunicationCommand.Subject, composeCommunicationCommand.MessageText, toUser, composeCommunicationCommand.IsGroup, composeCommunicationCommand.FromDate, composeCommunicationCommand.TemplateId, composeCommunicationCommand.CommunicationType, fromUser, composeCommunicationCommand.UserId);
                     
                return new RequestSaveEnvelop(response, "Request process successfully", null);
            }
            catch (System.Exception e)
            {
                var errorMessage = e.Message;
                Error error = new Error(ErrorType.BAD_REQUEST, errorMessage);
                return new RequestSaveEnvelop(false, string.Empty, error);
            }
        }

        private async Task<bool> SendEmail(string subject,string messageText, UserContactDTO fromUser, UserContactDTO toUser)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(fromUser.UserFullName, fromUser.Email));
            message.To.Add(new MailboxAddress(toUser.UserFullName, toUser.Email));
            message.Subject = subject;
            
            message.Body = new TextPart("html")
            {
                Text= messageText
            };
            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect("mail.parentcheck.lk", 8889, false);
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

        private async Task<bool> SendSms(string subject, string messageText, UserContactDTO fromUser, UserContactDTO toUser)
        {
            var message = $"{subject} - {messageText}";
            string sURL = $"https://cloud.websms.lk/smsAPI?sendsms&apikey=38wtZmSpjMOoJqsI87ag0XHGd8ZrrAMj&apitoken=5B1V1626690414&type=sms&from=Parentcheck&to={toUser.Mobile}&text={message}";
            try
            {
                using (WebClient client = new WebClient())
                {
                    string s = client.DownloadString(sURL);

                    var responseObject = Newtonsoft.Json.JsonConvert.DeserializeObject<Response>(s);
                    if(responseObject.Status== "queued")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
