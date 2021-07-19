using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common.Models;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using Serilog;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly JwtService jwtservice;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ReferenceController(IMediator mediator, JwtService jwtservice, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            this.jwtservice = jwtservice;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("getReference")]
        public async Task<JsonResult> GetReferenceByType(int id)
        {
            int instituteUserId = 1;
            
            var events = await mediator.Send((IRequest<ReferenceEnvelop>)new ReferenceQuery(id, instituteUserId));

            var response= ReferenceResponses.PopulateReferenceResponses(events.References);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getUserContacts")]
        public async Task<JsonResult> GetUserContacts(string name)
        {
            int instituteUserId = 1;

            var userContacts = await mediator.Send((IRequest<UserContactEnvelop>)new UserContactQuery(name, instituteUserId));

            var response = UserContactResponses.PopulateUserContactsResponses(userContacts.UserContacts);

            return new JsonResult(response);
        }

    }
}
