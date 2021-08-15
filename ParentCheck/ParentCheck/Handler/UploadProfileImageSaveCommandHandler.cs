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
    public class UploadProfileImageSaveCommandHandler : IRequestHandler<UploadProfileImageSaveCommand, RequestSaveEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public UploadProfileImageSaveCommandHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<RequestSaveEnvelop> Handle(UploadProfileImageSaveCommand uploadProfileImageSaveCommand, CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            try
            {
                var response = await settingDomain.UploadProfileImage(uploadProfileImageSaveCommand.EncryptedFileName, uploadProfileImageSaveCommand.UploadPath, uploadProfileImageSaveCommand.FileName, uploadProfileImageSaveCommand.UserId);

                if (!response)
                {
                    var errorMessage = "Request fail due to invalid user";
                    Error error = new Error(ErrorType.UNAUTHORIZED, errorMessage);
                    return new RequestSaveEnvelop(false, string.Empty, error);
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
    }
}
