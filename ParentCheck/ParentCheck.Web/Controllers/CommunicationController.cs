using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Common;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common;
using ParentCheck.Web.Common.Models;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using ParentCheck.Web.Models;
using System;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly JwtService jwtservice;

        public CommunicationController(IMediator mediator, JwtService jwtservice)
        {
            this.mediator = mediator;
            this.jwtservice = jwtservice;
        }

        [HttpGet]
        [Route("getCommunicationInbox")]
        public async Task<JsonResult> GetCommunicationInbox()
        {
            int userId = 1;

            var userCommunication = await mediator.Send((IRequest<UserCommunicationEnvelop>)new UserCommunicationInboxQuery(userId));

            var response = CommunicationResponses.PopulateCommunicationResponses(userCommunication.Communications);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getCommunicationOutbox")]
        public async Task<JsonResult> GetCommunicationOutbox()
        {
            int userId = 1;

            var userCommunication = await mediator.Send((IRequest<UserCommunicationEnvelop>)new UserCommunicationOutboxQuery(userId));

            var response = CommunicationResponses.PopulateCommunicationResponses(userCommunication.Communications);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getDetailCommunication")]
        public async Task<JsonResult> GetCommunicationDetail(long id,int type)
        {
            int userId = 1;

            var userCommunication = await mediator.Send((IRequest<UserCommunicationDetailEnvelop>)new UserCommunicationDetailQuery(id,type, userId));

            var response = CommunicationDetailResponses.PopulateCommunicationDetailResponses(userCommunication.Communication);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("composeCommunication")]
        public async Task<JsonResult> ComposeCommunication()
        {
            int userId = 1;

            var userCommunication = await mediator.Send((IRequest<UserCommunicationDetailEnvelop>)new UserCommunicationDetailQuery(0, 0, userId));

            var response = CommunicationDetailResponses.PopulateCommunicationDetailResponses(userCommunication.Communication);

            return new JsonResult(response);
        }
    }
}
