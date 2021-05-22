using MediatR;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator mediator;

        public RoleController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<RoleEnvelop>> Get()
        {
            var test = await mediator.Send((IRequest<RoleEnvelop>)new PackageQuery());

            return new ApiResponse<RoleEnvelop>(test);
        }
    }
}
