using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Envelope;
using ParentCheck.Query;
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
    public class ClassRoomController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly JwtService jwtservice;
        private readonly IHttpContextAccessor httpContextAccessor;

        public ClassRoomController(IMediator mediator, JwtService jwtservice, IHttpContextAccessor httpContextAccessor)
        {
            this.mediator = mediator;
            this.jwtservice = jwtservice;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        [Route("userSubjects")]
        public async Task<JsonResult> GetUserSubject()
        {
            int userId = 1;
            
            var events = await mediator.Send((IRequest<UserSubjectEnvelop>)new UserSubjectQuery(userId));

            var response= SubjectResponses.PopulateSubjectResponses(events.UserClass);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("subjectChapter")]
        public async Task<JsonResult> GetSubjectChapter(int id)
        {
            int userId = 1;

            var events = await mediator.Send((IRequest<UserSubjectChapterEnvelop>)new UserSubjectChapterQuery(id,userId));

            var response = SubjectChapterResponses.PopulateSubjectChapterResponses(events.UserSubjectChapter);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("chapterTopics")]
        public async Task<JsonResult> GetChapterTopics(int id)
        {
            int userId = 1;

            var events = await mediator.Send((IRequest<UserChapterTopicsEnvelop>)new UserChapterTopicQuery(id, userId));

            var response = ChapterTopicsResponses.PopulateChapterTopicsResponses(events.UserChapterTopics);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("topicsContent")]
        public async Task<JsonResult> GetTopicContent(int id)
        {
            int userId = 1;

            var events = await mediator.Send((IRequest<UserTopicContentEnvelop>)new UserTopicContentsQuery(id, userId));

            var response = TopicContentResponses.PopulateChapterTopicsResponses(events.UserTopicContents);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getSubmitedAssignmentFile")]
        public async Task<JsonResult> GetSubmitedAssignmentFileByAssignmentId(int id)
        {
            int userId = 1;

            var events = await mediator.Send((IRequest<UserSubmitedAssignmentFileEnvelop>)new UserSubmitedAssignmentFileQuery(id, userId));

            var response = UserSubmitedAssignmentFileResponses.PopulateSubmitedAssignmentFileResponses(events.SubmitedAssignmentFile);

            return new JsonResult(response);
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadAssignmentFile")]
        public async Task<IActionResult> UploadAssignmentFile()
        {
            int userId = 1;

            var file = Request.Form.Files[0];
            var assignmentId = Request.Headers["assignmentId"];

            if (string.IsNullOrEmpty(assignmentId))
            {
                return BadRequest(new JsonResult("Invalid Assignment Id"));
            }

            var savePath = $"upload/{userId}/assignment/{assignmentId}";
            var uploadPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "storage", savePath);

            var encryptedFileName = await FileUpload.FileUploadToServer(uploadPath, file);

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new UploadAssignmentFileCommand(long.Parse(assignmentId), encryptedFileName, savePath, file.FileName, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpPost]
        [Route("removeAssignmentFile")]
        public async Task<IActionResult> RemoveAssignmentFile(SubmissionDocument submissionDocument)
        {
            int userId = 1;
            
            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new AssignmentFileRemoveCommand(submissionDocument.submissionId,submissionDocument.id, userId));

            if (result.Created)
            {
                var savePath = $"upload/{userId}/assignment/{result.Id}";
                var uploadPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "storage", savePath, submissionDocument.enFileName);
                await FileUpload.FileDeleteFromServer(uploadPath);

                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(string.Empty));
        }

        [HttpPost]
        [Route("completeAssignment")]
        public async Task<IActionResult> CompleteAssignment(Assignment assignment)
        {
            int userId = 1;

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new CompleteAssignmentCommand(assignment.id, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(string.Empty));
        }
    }
}
