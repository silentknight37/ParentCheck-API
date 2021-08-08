using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Common;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common.Models;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : BaseController
    {
        private readonly IMediator mediator;

        public SettingController(IMediator mediator, JwtService jwtservice):base(jwtservice)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("getCommunicationTemplate")]
        public async Task<JsonResult> GetCommunicationTemplate()
        {
            var userId = GetUserIdFromToken();

            var communicationTemplates = await mediator.Send((IRequest<CommunicationTemplateEnvelop>)new CommunicationTemplateQuery(true, userId));

            var response = CommunicationTemplateResponses.PopulateCommunicationTemplateResponseResponses(communicationTemplates.CommunicationTemplates);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveCommunicationTemplate")]
        public async Task<IActionResult> SaveCommunicationTemplate([FromBody] CommunicationTemplateRequest communicationTemplateRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new CommunicationTemplateCommand(communicationTemplateRequest.Id, communicationTemplateRequest.Name, communicationTemplateRequest.Content, communicationTemplateRequest.IsSenderTemplate, communicationTemplateRequest.IsActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }
    }
}
