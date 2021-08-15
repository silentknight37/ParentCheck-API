using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Common;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common.Models;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using System.IO;
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
        [Route("saveUsers")]
        public async Task<IActionResult> SaveUsers([FromBody] UserSaveRequest userSaveRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new UserSaveCommand(userSaveRequest.id, userSaveRequest.firstName, userSaveRequest.lastName, string.Empty, userSaveRequest.roleId, userSaveRequest.parentUserid, userSaveRequest.username, userSaveRequest.dateOfBirth, userSaveRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpPost]
        [Route("saveDriviceToken")]
        public async Task<IActionResult> SaveDriviceToken([FromBody] DeviceTokenRequest deviceTokenRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new DeviceTokenSaveCommand(deviceTokenRequest.deviceToken, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadProfilePic")]
        public async Task<IActionResult> UploadProfilePic()
        {
            var userId = GetUserIdFromToken();

            var file = Request.Form.Files[0];


            var savePath = $"upload/profile";
            var uploadPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "storage", savePath);

            var userEnvelop = await mediator.Send((IRequest<UserEnvelop>)new UserQuery(userId));
            if (userEnvelop.User != null && !string.IsNullOrEmpty(userEnvelop.User.FileName))
            {
                var deletePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "storage", savePath, userEnvelop.User.FileName);
                await FileUpload.FileDeleteFromServer(deletePath);
            }
            var encryptedFileName = await FileUpload.FileUploadToServer(uploadPath, file);

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new UploadProfileImageSaveCommand(encryptedFileName, savePath, file.FileName, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }
        [HttpGet]
        [Route("getPerformance")]
        public async Task<JsonResult> GetPerformance()
        {
            var userId = GetUserIdFromToken();

            var performanceEnvelop = await mediator.Send((IRequest<PerformanceEnvelop>)new PerformanceQuery(userId));

            var response = PerformanceResponses.PopulatePerformanceResponses(performanceEnvelop.performances);

            return new JsonResult(response);
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

        [HttpGet]
        [Route("getClassSubject")]
        public async Task<JsonResult> GetClassSubject(long classId)
        {
            var userId = GetUserIdFromToken();

            var studentEnrollEnvelop = await mediator.Send((IRequest<AcademicClassSubjectEnvelop>)new ClassSubjectQuery(classId, userId));

            var response = AcademicClassSubjectResponses.PopulateAcademicClassSubjectResponses(studentEnrollEnvelop.academicClassSubjects);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveClassSubject")]
        public async Task<IActionResult> SaveClassSubject([FromBody] AcademicClassSubjectRequest academicClassSubjectRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new AcademicClassSubjectSaveCommand(academicClassSubjectRequest.id, academicClassSubjectRequest.classId, academicClassSubjectRequest.subjectId, academicClassSubjectRequest.responsibleUserId, academicClassSubjectRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpGet]
        [Route("getSubjectChapter")]
        public async Task<JsonResult> GetSubjectChapter(long subjectId)
        {
            var userId = GetUserIdFromToken();

            var subjectChapterEnvelop = await mediator.Send((IRequest<AcademicSubjectChapterEnvelop>)new SubjectChapterQuery(subjectId, userId));

            var response = AssociateSubjectChapterResponses.PopulateAssociateSubjectChapterResponses(subjectChapterEnvelop.subjectChapters);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveSubjectChapter")]
        public async Task<IActionResult> SaveSubjectChapter([FromBody] SubjectChapterRequest subjectChapterRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new SubjectChapterSaveCommand(subjectChapterRequest.id, subjectChapterRequest.subjectId, subjectChapterRequest.chapter, subjectChapterRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpGet]
        [Route("getChapterTopic")]
        public async Task<JsonResult> GetChapterTopic(long chapterId)
        {
            var userId = GetUserIdFromToken();

            var chapterTopicEnvelop = await mediator.Send((IRequest<AcademicChapterTopicEnvelop>)new ChapterTopicQuery(chapterId, userId));

            var response = AssociateChapterTopicResponses.PopulateAssociateChapterTopicResponses(chapterTopicEnvelop.chapterTopics);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveChapterTopic")]
        public async Task<IActionResult> SaveChapterTopic([FromBody] ChapterTopicRequest chapterTopicRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new ChapterTopicSaveCommand(chapterTopicRequest.id, chapterTopicRequest.chapterId, chapterTopicRequest.topic, chapterTopicRequest.description, chapterTopicRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpGet]
        [Route("getTopicContent")]
        public async Task<JsonResult> GetTopicContent(long topicId)
        {
            var userId = GetUserIdFromToken();

            var topicContentEnvelop = await mediator.Send((IRequest<AcademicTopicContentEnvelop>)new TopicContentQuery(topicId, userId));

            var response = AssociateTopicContentResponses.PopulateAssociateTopicContentResponses(topicContentEnvelop.topicContents);

            return new JsonResult(response);
        }

        [HttpPost]
        [Route("saveTopicContent")]
        public async Task<IActionResult> SaveTopicContent([FromBody] TopicContentRequest topicContentRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new TopicContentSaveCommand(topicContentRequest.id, topicContentRequest.topicId, topicContentRequest.contentText, topicContentRequest.contentTypeId, topicContentRequest.orderId, topicContentRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("uploadContentFile")]
        public async Task<IActionResult> UploadContentFile()
        {
            var userId = GetUserIdFromToken();

            var file = Request.Form.Files[0];
            var topicContentId = Request.Headers["topicContentId"];
            var chapterTopicId = Request.Headers["chapterTopicId"];
            var subjectChapterId = Request.Headers["subjectChapterId"];

            if (string.IsNullOrEmpty(topicContentId) || string.IsNullOrEmpty(chapterTopicId) || string.IsNullOrEmpty(subjectChapterId))
            {
                return BadRequest(new JsonResult("Invalid Request Process"));
            }

            var savePath = $"upload/content/{subjectChapterId}/{chapterTopicId}/{topicContentId}";
            var uploadPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "storage", savePath);

            var encryptedFileName = await FileUpload.FileUploadToServer(uploadPath, file);

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new UploadContentFileCommand(long.Parse(topicContentId), encryptedFileName, savePath, file.FileName, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpPost]
        [Route("removeContentFile")]
        public async Task<IActionResult> RemoveContentFile(TopicContentFileRemoveRequest topicContentFileRemoveRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new TopicContentFileRemoveCommand(topicContentFileRemoveRequest.topicContentId, topicContentFileRemoveRequest.id, userId));

            if (result.Created)
            {
                var savePath = $"upload/content/{topicContentFileRemoveRequest.subjectChapterId}/{topicContentFileRemoveRequest.chapterTopicId}/{topicContentFileRemoveRequest.topicContentId}";
                var uploadPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "storage", savePath, topicContentFileRemoveRequest.enFileName);
                await FileUpload.FileDeleteFromServer(uploadPath);

                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(string.Empty));
        }


        [HttpGet]
        [Route("getAssociateClass")]
        public async Task<JsonResult> GetAssociateClass()
        {
            var userId = GetUserIdFromToken();

            var associateClassEnvelop = await mediator.Send((IRequest<AssociateClassEnvelop>)new AssociateClassQuery(userId));

            var response = AssociateClassResponses.PopulateAssociateClassResponses(associateClassEnvelop.associateClasses);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getTimeTable")]
        public async Task<JsonResult> GetTimeTable()
        {
            var userId = GetUserIdFromToken();

            var timeTableEnvelop = await mediator.Send((IRequest<TimeTableEnvelop>)new TimeTableQuery(false,userId));

            var response = TimeTableResponses.PopulateTimeTableResponses(timeTableEnvelop.weekDays);

            return new JsonResult(response);
        }

        [HttpGet]
        [Route("getTodayTimeTable")]
        public async Task<JsonResult> GetTodayTimeTable()
        {
            var userId = GetUserIdFromToken();

            var timeTableEnvelop = await mediator.Send((IRequest<TimeTableEnvelop>)new TimeTableQuery(true, userId));

            var response = TimeTableResponses.PopulateTimeTableResponses(timeTableEnvelop.weekDays);

            return new JsonResult(response);
        }
    }
}
