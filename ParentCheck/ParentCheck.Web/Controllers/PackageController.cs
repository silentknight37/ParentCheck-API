using MediatR;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common;
using ParentCheck.Web.Helpers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ParentCheck.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly JwtService jwtservice;

        public PackageController(IMediator mediator,JwtService jwtservice)
        {
            this.mediator = mediator;
            this.jwtservice = jwtservice;
        }

        [HttpGet]
        public async Task<ApiResponse<PackageEnvelop>> Get()
        {
            //var test = await mediator.Send((IRequest<PackageEnvelop>)new PackageQuery());

            //var jwt = jwtservice.Generate(1);

            //Response.Cookies.Append("jwt", jwt,new CookieOptions { 
            //    HttpOnly=true
            //});

            return new ApiResponse<PackageEnvelop>(null);
        }
    }
}
