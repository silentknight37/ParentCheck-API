using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReferenceController : BaseController
    {
        private readonly IMediator mediator;

        public ReferenceController(IMediator mediator, JwtService jwtservice, IHttpContextAccessor httpContextAccessor) : base(jwtservice)
        {
            this.mediator = mediator;
        }
                
        [HttpGet]
        [Route("getReference")]
        public async Task<JsonResult> GetReferenceByType(int id, long? contextId)
        {
            var userId = GetUserIdFromToken();

            var events = await mediator.Send((IRequest<ReferenceEnvelop>)new ReferenceQuery(contextId, id, userId));

            var response= ReferenceResponses.PopulateReferenceResponses(events.References);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getUserContacts")]
        public async Task<JsonResult> GetUserContacts(string name)
        {
            var userId = GetUserIdFromToken();

            var userContacts = await mediator.Send((IRequest<UserContactEnvelop>)new UserContactQuery(name, userId));

            var response = UserContactResponses.PopulateUserContactsResponses(userContacts.UserContacts);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getAllUserContacts")]
        public async Task<JsonResult> GetAllUserContacts(int sendType)
        {
            var userId = GetUserIdFromToken();

            var userContacts = await mediator.Send((IRequest<UserContactEnvelop>)new UserAllContactQuery(sendType, userId));

            var response = UserContactResponses.PopulateUserContactsResponses(userContacts.UserContacts);

            return new JsonResult(response);
        }

    }
}
