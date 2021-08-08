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
    public class SupportController : BaseController
    {
        private readonly IMediator mediator;

        public SupportController(IMediator mediator, JwtService jwtservice):base(jwtservice)
        {
            this.mediator = mediator;
        }

        
        [HttpPost]
        [Route("newSupport")]
        public async Task<IActionResult> NewSupportTicket(NewSupportTicket newSupportTicket)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new NewSupportTicketSaveCommand(newSupportTicket.Subject, newSupportTicket.IssueText, userId));

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
            var userId = GetUserIdFromToken();

            var events = await mediator.Send((IRequest<SupportTicketEnvelop>)new UserSupportTicketQuery(EnumSupportTicketType.Open, userId));

            var response = SupportTicketResponses.PopulateSupportTicketResponses(events.SupportTickets);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getCloseTickets")]
        public async Task<JsonResult> GetCloseTickets()
        {
            var userId = GetUserIdFromToken();

            var events = await mediator.Send((IRequest<SupportTicketEnvelop>)new UserSupportTicketQuery(EnumSupportTicketType.Closed, userId));

            var response = SupportTicketResponses.PopulateSupportTicketResponses(events.SupportTickets);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getAssignTickets")]
        public async Task<JsonResult> GetAssignTickets()
        {
            var userId = GetUserIdFromToken();

            var events = await mediator.Send((IRequest<SupportTicketEnvelop>)new UserSupportTicketQuery(EnumSupportTicketType.Review, userId));

            var response = SupportTicketResponses.PopulateSupportTicketResponses(events.SupportTickets);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getDetailTickets")]
        public async Task<JsonResult> GetDetailTickets(long id)
        {
            var userId = GetUserIdFromToken();

            var events = await mediator.Send((IRequest<DetailTicketEnvelop>)new UserDetailTicketQuery(id, userId));

            var response = DetailSupportTicketResponses.PopulateDetailSupportTicketResponses(userId,events.SupportTicket);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("replySupport")]
        public async Task<IActionResult> ReplySupportTicket(ReplySupportTicket replySupportTicket)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new SupportTicketReplyCommand(replySupportTicket.TicketId, replySupportTicket.ReplyMessage, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(string.Empty));
        }
    }
}
