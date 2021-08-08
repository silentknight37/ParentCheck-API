using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using System;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CalenderController : BaseController
    {
        private readonly IMediator mediator;

        public CalenderController(IMediator mediator, JwtService jwtservice) :base(jwtservice)
        {
            this.mediator = mediator;
        }
        
        [HttpGet]
        [Route("event")]
        public async Task<JsonResult> GetCalenderEvent(DateTime? requestedDate,int eventType)
        {
            var userId = GetUserIdFromToken();
            
            var eventRequestedDate = requestedDate != null ? requestedDate.Value : DateTime.UtcNow;
            var events = await mediator.Send((IRequest<CalenderEventEnvelop>)new CalenderEventQuery(eventRequestedDate, eventType, userId));

            var response=CalenderEventResponses.PopulateCalenderEventResponses(events.CalenderEvents);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("eventCreate")]
        public async Task<IActionResult> EventCreate(CalenderEvent calenderEvent)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new CalenderEventSaveCommand(calenderEvent.fromDate, calenderEvent.toDate, calenderEvent.subject, calenderEvent.description, calenderEvent.type, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpPost]
        [Route("eventRemove")]
        public async Task<IActionResult> EventRemove(CalenderEvent calenderEvent)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new CalenderEventRemoveCommand(calenderEvent.id, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }
    }
}
