using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.BusinessObject;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common.Models;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : BaseController
    {
        private readonly IMediator mediator;
        private readonly IHttpContextAccessor httpContextAccessor;

        public PaymentController(IMediator mediator, JwtService jwtservice, IHttpContextAccessor httpContextAccessor):base(jwtservice)
        {
            this.mediator = mediator;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("getUserInvoices")]
        public async Task<JsonResult> GetUserInvoices()
        {
            var userId = GetUserIdFromToken();

            var data = await mediator.Send((IRequest<InvoiceEnvelop>)new InvoiceQuery(false, userId));

            var response= InvoiceResponses.PopulateInvoiceResponses(data.Invoices);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getUserInvoiceDetail")]
        public async Task<JsonResult> GetUserInvoiceDetail(long id)
        {
            var userId = GetUserIdFromToken();

            var data = await mediator.Send((IRequest<InvoiceDetailEnvelop>)new InvoiceDetailQuery(false,id, userId));

            var response = InvoiceDetailResponses.PopulateInvoiceDetailResponses(data.Invoice);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getGeneratedInvoices")]
        public async Task<JsonResult> GetGeneratedInvoices()
        {
            var userId = GetUserIdFromToken();

            var data = await mediator.Send((IRequest<InvoiceEnvelop>)new InvoiceQuery(true, userId));

            var response = InvoiceResponses.PopulateInvoiceResponses(data.Invoices);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getGeneratedInvoiceDetail")]
        public async Task<JsonResult> GetGeneratedInvoiceDetail(long id)
        {
            var userId = GetUserIdFromToken();

            var data = await mediator.Send((IRequest<InvoiceDetailEnvelop>)new InvoiceDetailQuery(true,id, userId));

            var response = InvoiceDetailResponses.PopulateInvoiceDetailResponses(data.Invoice);

            return new JsonResult(response);
        }


        [HttpPost]
        [Route("generateInvoice")]
        public async Task<IActionResult> GenerateInvoice(InvoiceGenerateRequest invoiceGenerateRequest)
        {
            var userId = GetUserIdFromToken();

            var toUser = new List<UserContactDTO>();
            foreach (var user in invoiceGenerateRequest.toUsers)
            {
                toUser.Add(new UserContactDTO
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    UserFullName = user.ToValue
                });
            }

            var toGroup = new List<ReferenceDTO>();
            foreach (var group in invoiceGenerateRequest.toGroups)
            {
                toGroup.Add(new ReferenceDTO
                {
                    Id = group.id,
                    ValueText = group.toValue
                });
            }

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new GenerateInvoiceCommand(invoiceGenerateRequest.invoiceTitle, invoiceGenerateRequest.invoiceDetails, toUser, toGroup, invoiceGenerateRequest.isGroup, invoiceGenerateRequest.dueDate, invoiceGenerateRequest.invoiceDate, invoiceGenerateRequest.invoiceAmount, invoiceGenerateRequest.invoiceTypeId, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpPost]
        [Route("payInvoice")]
        public async Task<IActionResult> PayInvoice(InvoiceGenerateRequest invoiceGenerateRequest)
        {
            var userId = GetUserIdFromToken();

            var toUser = new List<UserContactDTO>();
            foreach (var user in invoiceGenerateRequest.toUsers)
            {
                toUser.Add(new UserContactDTO
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    UserFullName = user.ToValue
                });
            }

            var toGroup = new List<ReferenceDTO>();
            foreach (var group in invoiceGenerateRequest.toGroups)
            {
                toGroup.Add(new ReferenceDTO
                {
                    Id = group.id,
                    ValueText = group.toValue
                });
            }

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new GenerateInvoiceCommand(invoiceGenerateRequest.invoiceTitle, invoiceGenerateRequest.invoiceDetails, toUser, toGroup, invoiceGenerateRequest.isGroup, invoiceGenerateRequest.dueDate, invoiceGenerateRequest.invoiceDate, invoiceGenerateRequest.invoiceAmount, invoiceGenerateRequest.invoiceTypeId, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpGet]
        [Route("getInvoiceTypes")]
        public async Task<JsonResult> GetInvoiceTypes()
        {
            var userId = GetUserIdFromToken();

            var data = await mediator.Send((IRequest<InvoiceTypeEnvelop>)new InvoiceTypeQuery(userId));

            var response = InvoiceTypeResponses.PopulateInvoiceTypeResponses(data.InvoiceTypes);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveInvoiceType")]
        public async Task<IActionResult> SaveInvoiceType(InvoiceTypeRequest invoiceTypeRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new InvoiceTypeSaveCommand(invoiceTypeRequest.Id, invoiceTypeRequest.TypeText, invoiceTypeRequest.Terms, invoiceTypeRequest.IsActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }
    }
}
