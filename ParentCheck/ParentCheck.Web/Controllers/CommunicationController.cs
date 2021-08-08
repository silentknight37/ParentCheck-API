using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.BusinessObject;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common.Models;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationController : BaseController
    {
        private readonly IMediator mediator;

        public CommunicationController(IMediator mediator, JwtService jwtservice):base(jwtservice)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("getCommunicationInbox")]
        public async Task<JsonResult> GetCommunicationInbox()
        {
            var userId = GetUserIdFromToken();

            var userCommunication = await mediator.Send((IRequest<UserCommunicationEnvelop>)new UserCommunicationInboxQuery(userId));

            var response = CommunicationResponses.PopulateCommunicationResponses(userCommunication.Communications);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getCommunicationOutbox")]
        public async Task<JsonResult> GetCommunicationOutbox()
        {
            var userId = GetUserIdFromToken();

            var userCommunication = await mediator.Send((IRequest<UserCommunicationEnvelop>)new UserCommunicationOutboxQuery(userId));

            var response = CommunicationResponses.PopulateCommunicationResponses(userCommunication.Communications);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getSmsCommunicationOutbox")]
        public async Task<JsonResult> GetSmsCommunicationOutbox()
        {
            var userId = GetUserIdFromToken();

            var userCommunication = await mediator.Send((IRequest<UserCommunicationEnvelop>)new UserSmsCommunicationOutboxQuery(userId));

            var response = CommunicationResponses.PopulateCommunicationResponses(userCommunication.Communications);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getDetailCommunication")]
        public async Task<JsonResult> GetCommunicationDetail(long id,int type)
        {
            var userId = GetUserIdFromToken();

            var userCommunication = await mediator.Send((IRequest<UserCommunicationDetailEnvelop>)new UserCommunicationDetailQuery(id,type, userId));

            var response = CommunicationDetailResponses.PopulateCommunicationDetailResponses(userCommunication.Communications);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("composeCommunication")]
        public async Task<IActionResult> ComposeCommunication([FromBody]ComposeCommunicationRequest composeCommunicationRequest)
        {
            var userId = GetUserIdFromToken();
            var toUser = new List<UserContactDTO>();
            foreach (var user in composeCommunicationRequest.ToUsers)
            {
                toUser.Add(new UserContactDTO
                {
                    UserId= user.Id,
                    Email= user.Email,
                    Mobile= user.Mobile,
                    UserFullName= user.FullName
                });
            }

            var toGroup = new List<ReferenceDTO>();
            foreach (var group in composeCommunicationRequest.ToGroups)
            {
                toGroup.Add(new ReferenceDTO
                {
                    Id = group.id,
                    ValueText = group.value
                });
            }

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new ComposeCommunicationCommand(composeCommunicationRequest.Subject, composeCommunicationRequest.MessageText, toUser, toGroup, composeCommunicationRequest.IsGroup, composeCommunicationRequest.FromDate, composeCommunicationRequest.ToDate, composeCommunicationRequest.TemplateId, composeCommunicationRequest.CommunicationType, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpPost]
        [Route("replyCommunication")]
        public async Task<IActionResult> ReplyCommunication([FromBody] ReplyCommunicationRequest replyCommunicationRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new ReplyCommunicationCommand(replyCommunicationRequest.Id,replyCommunicationRequest.Subject, replyCommunicationRequest.MessageText, replyCommunicationRequest.ToUserId, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpGet]
        [Route("getCommunicationTemplate")]
        public async Task<JsonResult> GetCommunicationTemplate()
        {
            var userId = GetUserIdFromToken();

            var communicationTemplates = await mediator.Send((IRequest<CommunicationTemplateEnvelop>)new CommunicationTemplateQuery(true,userId));

            var response = CommunicationTemplateResponses.PopulateCommunicationTemplateResponseResponses(communicationTemplates.CommunicationTemplates);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getAllCommunicationTemplate")]
        public async Task<JsonResult> GetAllCommunicationTemplate()
        {
            var userId = GetUserIdFromToken();

            var communicationTemplates = await mediator.Send((IRequest<CommunicationTemplateEnvelop>)new CommunicationTemplateQuery(false,userId));

            var response = CommunicationTemplateResponses.PopulateCommunicationTemplateResponseResponses(communicationTemplates.CommunicationTemplates);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveCommunicationTemplate")]
        public async Task<IActionResult> SaveCommunicationTemplate([FromBody] CommunicationTemplateRequest communicationTemplateRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new CommunicationTemplateCommand(communicationTemplateRequest.Id, communicationTemplateRequest.Name, communicationTemplateRequest.Content, communicationTemplateRequest.IsSenderTemplate, communicationTemplateRequest.IsActive,userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }
    }
}
