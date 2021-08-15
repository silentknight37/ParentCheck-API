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
    public class AcademicTermSaveCommandHandler : IRequestHandler<AcademicTermSaveCommand, RequestSaveEnvelop>
    {
        private readonly ISettingFactory settingFactory;

        public AcademicTermSaveCommandHandler(ParentCheckContext parentcheckContext)
        {
            this.settingFactory = new SettingFactory(parentcheckContext);
        }

        public async Task<RequestSaveEnvelop> Handle(AcademicTermSaveCommand academicTermSaveCommand, CancellationToken cancellationToken)
        {
            var settingDomain = this.settingFactory.Create();
            try
            {
                var response = await settingDomain.SaveAcademicTerm(academicTermSaveCommand.Id, academicTermSaveCommand.Term, academicTermSaveCommand.YearAcademicId, academicTermSaveCommand.FromDate, academicTermSaveCommand.ToDate, academicTermSaveCommand.IsActive, academicTermSaveCommand.UserId);

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
