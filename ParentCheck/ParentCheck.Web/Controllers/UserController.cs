using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common;
using ParentCheck.Web.Helpers;
using ParentCheck.Web.Models;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly JwtService jwtservice;

        public UserController(IMediator mediator, JwtService jwtservice)
        {
            this.mediator = mediator;
            this.jwtservice = jwtservice;
        }

        [HttpGet]
        public async Task<ApiResponse<UserEnvelop>> Get()
        {
            var test = await mediator.Send((IRequest<UserEnvelop>)new UserQuery());

            return new ApiResponse<UserEnvelop>(test);
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<ApiResponse<UserEnvelop>> Authenticate(AuthenticationDTO authenticationDTO)
        {
            var test = await mediator.Send((IRequest<UserEnvelop>)new UserQuery());
            var jwt = jwtservice.Generate(1);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });
            return new ApiResponse<UserEnvelop>(test);
        }
    }
}
