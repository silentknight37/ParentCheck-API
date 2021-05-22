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
    public class PackageController : ControllerBase
    {
        private readonly IMediator mediator;

        public PackageController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<PackageEnvelop>> Get()
        {
            var test = await mediator.Send((IRequest<PackageEnvelop>)new PackageQuery());

            return new ApiResponse<PackageEnvelop>(test);
        }
    }
}
