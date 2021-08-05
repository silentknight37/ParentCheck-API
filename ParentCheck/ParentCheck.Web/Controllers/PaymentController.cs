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
    public class PaymentController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly JwtService jwtservice;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PaymentController(IMediator mediator, JwtService jwtservice, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            this.jwtservice = jwtservice;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("getInvoiceRecord")]
        public async Task<JsonResult> GetInvoiceRecord()
        {
            int instituteUserId = 1;
            
            var data = await mediator.Send((IRequest<InvoiceEnvelop>)new InvoiceQuery(instituteUserId));

            var response= InvoiceResponses.PopulateInvoiceResponses(data.Invoices);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getInvoiceDetailRecord")]
        public async Task<JsonResult> GetInvoiceDetailRecord(long id)
        {
            int instituteUserId = 1;

            var data = await mediator.Send((IRequest<InvoiceEnvelop>)new InvoiceQuery(instituteUserId));

            var response = InvoiceResponses.PopulateInvoiceResponses(data.Invoices);

            return new JsonResult(response);
        }

    }
}
