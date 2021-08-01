using MediatR;
using ParentCheck.Common;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Factory;
using ParentCheck.Factory.Intreface;
using ParentCheck.Query;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ParentCheck.Handler
{
    public class SaveCommunicationTemplateCommandHandler : IRequestHandler<CommunicationTemplateCommand, RequestSaveEnvelop>
    {
        private readonly ICommunicationFactory communicationFactory;

        public SaveCommunicationTemplateCommandHandler(ParentCheckContext parentcheckContext)
        {
            this.communicationFactory = new CommunicationFactory(parentcheckContext);
        }

        public async Task<RequestSaveEnvelop> Handle(CommunicationTemplateCommand communicationTemplateCommand,CancellationToken cancellationToken)
        {
            var communicationDomain = this.communicationFactory.Create();
            try
            {
                var response = await communicationDomain.SaveCommunicationTemplate(communicationTemplateCommand.Id, communicationTemplateCommand.Name, communicationTemplateCommand.Content, communicationTemplateCommand.IsSenderTemplate, communicationTemplateCommand.IsActive, communicationTemplateCommand.UserId);
               
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
                return new RequestSaveEnvelop(false,string.Empty, error);
            }
        }
    }
}
