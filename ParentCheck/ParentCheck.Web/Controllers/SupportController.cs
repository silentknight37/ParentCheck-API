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
    public class SupportController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly JwtService jwtservice;

        public SupportController(IMediator mediator, JwtService jwtservice)
        {
            this.mediator = mediator;
            this.jwtservice = jwtservice;
        }

        [HttpPost]
        [Route("newSupport")]
        public async Task<IActionResult> NewSupportTicket(NewSupportTicket newSupportTicket)
        {
            int instituteUserId = 1;

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new NewSupportTicketSaveCommand(newSupportTicket.Subject, newSupportTicket.IssueText, instituteUserId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(string.Empty));
        }

        [HttpGet]
        [Route("getOpenTickets")]
        public async Task<JsonResult> GetOpenTickets()
        {
            int userId =1;

            var events = await mediator.Send((IRequest<SupportTicketEnvelop>)new UserSupportTicketQuery(EnumSupportTicketType.Open, userId));

            var response = SupportTicketResponses.PopulateSupportTicketResponses(events.SupportTickets);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getCloseTickets")]
        public async Task<JsonResult> GetCloseTickets()
        {
            int userId = 1;

            var events = await mediator.Send((IRequest<SupportTicketEnvelop>)new UserSupportTicketQuery(EnumSupportTicketType.Closed, userId));

            var response = SupportTicketResponses.PopulateSupportTicketResponses(events.SupportTickets);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getAssignTickets")]
        public async Task<JsonResult> GetAssignTickets()
        {
            int userId = 1;

            var events = await mediator.Send((IRequest<SupportTicketEnvelop>)new UserSupportTicketQuery(EnumSupportTicketType.Review, userId));

            var response = SupportTicketResponses.PopulateSupportTicketResponses(events.SupportTickets);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getDetailTickets")]
        public async Task<JsonResult> GetDetailTickets(long id)
        {
            int userId = 1;

            var events = await mediator.Send((IRequest<DetailTicketEnvelop>)new UserDetailTicketQuery(id, userId));

            var response = DetailSupportTicketResponses.PopulateDetailSupportTicketResponses(userId,events.SupportTicket);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("replySupport")]
        public async Task<IActionResult> ReplySupportTicket(ReplySupportTicket replySupportTicket)
        {
            int instituteUserId = 1;

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new SupportTicketReplyCommand(replySupportTicket.TicketId, replySupportTicket.ReplyMessage, instituteUserId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(string.Empty));
        }
    }
}
