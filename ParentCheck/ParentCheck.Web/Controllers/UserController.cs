using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.BusinessObject;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common;
using ParentCheck.Web.Common.Responses;
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
            int userId = 1;
            var test = await mediator.Send((IRequest<UserEnvelop>)new UserQuery(userId,null,string.Empty,string.Empty));

            return new ApiResponse<UserEnvelop>(test);
        }

        [HttpPost]
        [Route("authenticate")]
        public async Task<JsonResult> Authenticate(AuthenticationDTO authenticationDTO) 
        {
            UserDTO user=new UserDTO();
            var userEnvelop = await mediator.Send((IRequest<UserEnvelop>)new UserAuthenticateQuery(authenticationDTO.Username, authenticationDTO.Password));

            if (userEnvelop==null || userEnvelop.User == null)
            {
                user.IsValidUser = false;
                var responseError = UserResponses.PopulateUserResponses(string.Empty, user);

                return new JsonResult(responseError);
            }
            var jwt = jwtservice.Generate(userEnvelop.User);

            Response.Cookies.Append("jwt", jwt, new CookieOptions
            {
                HttpOnly = true
            });
            var response = UserResponses.PopulateUserResponses(jwt, userEnvelop.User);

            return new JsonResult(response);
        }
    }
}
