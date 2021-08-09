using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Common;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common.Models;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : BaseController
    {
        private readonly IMediator mediator;

        public SettingController(IMediator mediator, JwtService jwtservice):base(jwtservice)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Route("getUsers")]
        public async Task<JsonResult> GeInstituteUsers()
        {
            var userId = GetUserIdFromToken();

            var instituteUsers = await mediator.Send((IRequest<InstituteUsersEnvelop>)new InstituteUsersQuery(userId));

            var response = InstituteUserResponses.PopulateInstituteUserResponses(instituteUsers.InstituteUsers);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveCommunicationTemplate")]
        public async Task<IActionResult> SaveCommunicationTemplate([FromBody] CommunicationTemplateRequest communicationTemplateRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new CommunicationTemplateCommand(communicationTemplateRequest.Id, communicationTemplateRequest.Name, communicationTemplateRequest.Content, communicationTemplateRequest.IsSenderTemplate, communicationTemplateRequest.IsActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpGet]
        [Route("getAcademicYear")]
        public async Task<JsonResult> GetAcademicYears()
        {
            var userId = GetUserIdFromToken();

            var academicEnvelop = await mediator.Send((IRequest<AcademicEnvelop>)new AcademicQuery(userId));

            var response = AcademicResponses.PopulateAcademicResponses(academicEnvelop.academics);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveAcademicYear")]
        public async Task<IActionResult> SaveAcademicYear([FromBody] AcademicRequest academicRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new AcademicSaveCommand(academicRequest.id, academicRequest.yearAcademic,academicRequest.fromDate, academicRequest.toDate, academicRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }


        [HttpGet]
        [Route("getAcademicTerm")]
        public async Task<JsonResult> GetAcademicTerms()
        {
            var userId = GetUserIdFromToken();

            var academicTermEnvelop = await mediator.Send((IRequest<AcademicTermEnvelop>)new AcademicTermQuery(userId));

            var response = AcademicTermResponses.PopulateAcademicTermResponses(academicTermEnvelop.academicTerms);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveAcademicTerm")]
        public async Task<IActionResult> SaveAcademicTerm([FromBody] AcademicTermRequest academicTermRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new AcademicTermSaveCommand(academicTermRequest.id, academicTermRequest.term, academicTermRequest.yearAcademic, academicTermRequest.fromDate, academicTermRequest.toDate, academicTermRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpGet]
        [Route("getClasses")]
        public async Task<JsonResult> GetClasses()
        {
            var userId = GetUserIdFromToken();

            var academicClassEnvelop = await mediator.Send((IRequest<AcademicClassEnvelop>)new AcademicClassQuery(userId));

            var response = AcademicClassResponses.PopulateAcademicClassResponses(academicClassEnvelop.academicClasses);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveClasses")]
        public async Task<IActionResult> SaveClasses([FromBody] AcademicClassRequest academicClassRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new AcademicClassSaveCommand(academicClassRequest.id, academicClassRequest.academicClass, academicClassRequest.yearAcademic, academicClassRequest.responsibleUserId, academicClassRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpGet]
        [Route("getSubject")]
        public async Task<JsonResult> GetSubject()
        {
            var userId = GetUserIdFromToken();

            var subjectEnvelop = await mediator.Send((IRequest<SubjectEnvelop>)new SubjectQuery(userId));

            var response = AcademicSubjectResponses.PopulateAcademicSubjectResponses(subjectEnvelop.subjects);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveSubject")]
        public async Task<IActionResult> SaveSubject([FromBody] AcademicSubjectRequest academicSubjectRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new AcademicSubjectSaveCommand(academicSubjectRequest.id, academicSubjectRequest.subject, academicSubjectRequest.descriptionText, academicSubjectRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpGet]
        [Route("getStudentEnroll")]
        public async Task<JsonResult> GetStudentEnroll(long classId, long academicYear)
        {
            var userId = GetUserIdFromToken();

            var studentEnrollEnvelop = await mediator.Send((IRequest<StudentEnrollEnvelop>)new StudentEnrollQuery(classId, academicYear, userId));

            var response = StudentEnrollResponses.PopulateStudentEnrollResponses(studentEnrollEnvelop.studentEnrolls);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveStudentEnroll")]
        public async Task<IActionResult> SaveStudentEnroll([FromBody] StudentEnrollRequest studentEnrollRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new StudentEnrollSaveCommand(studentEnrollRequest.id, studentEnrollRequest.academicYear, studentEnrollRequest.classId, studentEnrollRequest.studentId, studentEnrollRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

    }
}
