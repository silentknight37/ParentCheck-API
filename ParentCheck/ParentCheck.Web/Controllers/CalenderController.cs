using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using ParentCheck.Web.Models;
using System;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalenderController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly JwtService jwtservice;

        public CalenderController(IMediator mediator, JwtService jwtservice)
        {
            this.mediator = mediator;
            this.jwtservice = jwtservice;
        }

        [HttpGet]
        [Route("event")]
        public async Task<JsonResult> GetCalenderEvent(DateTime? requestedDate,int eventType)
        {
            int instituteUserId = 1;
            var eventRequestedDate = requestedDate != null ? requestedDate.Value : DateTime.UtcNow;
            var events = await mediator.Send((IRequest<CalenderEventEnvelop>)new CalenderEventQuery(eventRequestedDate, eventType, instituteUserId));

            var response=CalenderEventResponses.PopulateCalenderEventResponses(events.CalenderEvents);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("eventCreate")]
        public async Task<IActionResult> EventCreate(CalenderEvent calenderEvent)
        {
            int instituteUserId = 1;

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new CalenderEventSaveCommand(calenderEvent.fromDate, calenderEvent.toDate, calenderEvent.subject, calenderEvent.description, calenderEvent.type, instituteUserId));

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
            int instituteUserId = 1;

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new CalenderEventRemoveCommand(calenderEvent.id, instituteUserId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }
    }
}
