using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParentCheck.Common;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common.Models;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : BaseController
    {
        private readonly IMediator mediator;

        public SettingController(IMediator mediator, JwtService jwtservice) :base(jwtservice)
        {
            this.mediator = mediator;
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpGet]
        [Route("getUsers")]
        public async Task<JsonResult> GeInstituteUsers(string searchValue,int roleId)
        {
            var userId = GetUserIdFromToken();

            var instituteUsers = await mediator.Send((IRequest<InstituteUsersEnvelop>)new InstituteUsersQuery(searchValue,roleId,userId));

            var response = InstituteUserResponses.PopulateInstituteUserResponses(instituteUsers.InstituteUsers);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpGet]
        [Route("getUserById")]
        public async Task<JsonResult> GeInstituteUserbyId(long id)
        {
            var userId = GetUserIdFromToken();

            var instituteUsers = await mediator.Send((IRequest<InstituteUsersEnvelop>)new InstituteUsersQuery(string.Empty,null, userId));
            var user = instituteUsers.InstituteUsers.Where(i => i.UserId == id).ToList();
            var response = InstituteUserResponses.PopulateInstituteUserResponses(user);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpPost]
        [Route("saveUsers")]
        public async Task<IActionResult> SaveUsers([FromBody] UserSaveRequest userSaveRequest)
        {
            var userId = GetUserIdFromToken();
            DateTime? parentDateOfBirth = null;
            if (!string.IsNullOrEmpty(userSaveRequest.parentDateOfBirth))
            {
                parentDateOfBirth = StringToDate(userSaveRequest.parentDateOfBirth);
            }
            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new UserSaveCommand(
                userSaveRequest.id,
                userSaveRequest.firstName,
                userSaveRequest.lastName,
                string.Empty,
                string.Empty,
                userSaveRequest.roleId,
                userSaveRequest.username,
                StringToDate(userSaveRequest.dateOfBirth),
                parentDateOfBirth,
                userSaveRequest.admission,
                userSaveRequest.mobile,
                userSaveRequest.parentId,
                userSaveRequest.parentFirstName,
                userSaveRequest.parentLastName,
                userSaveRequest.parentUsername,
                userSaveRequest.parentMobile,
                userSaveRequest.isActive,
                userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [HttpPost]
        [Route("resetPasswordUsers")]
        public async Task<IActionResult> ResetPasswordUser([FromBody] ResetPasswordRequest resetPasswordRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new ResetPasswordCommand(
                resetPasswordRequest.id,
                userId));

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
            var savePath = $"upload/profile/";
            var file = Request.Form.Files[0];

            var user = await mediator.Send((IRequest<UserEnvelop>)new UserQuery(userId, null, string.Empty,string.Empty));
            if (user != null && !string.IsNullOrEmpty(user.User.EncryptedFileName))
            {
                var deletePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "storage", savePath, user.User.EncryptedFileName);
                await FileUpload.FileDeleteFromServer(deletePath);
            }

            var uploadPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).ToString(), "storage", savePath);

            var encryptedFileName = await FileUpload.FileUploadToServer(uploadPath, file);

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new UploadProfileImageSaveCommand(encryptedFileName, savePath, file.FileName, userId));

            if (result.Created)
            {
                return Ok(new JsonResult($"https://storage.parentcheck.lk/{savePath}/{encryptedFileName}"));
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

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpGet]
        [Route("getAcademicYear")]
        public async Task<JsonResult> GetAcademicYears()
        {
            var userId = GetUserIdFromToken();

            var academicEnvelop = await mediator.Send((IRequest<AcademicEnvelop>)new AcademicQuery(userId));

            var response = AcademicResponses.PopulateAcademicResponses(academicEnvelop.academics);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpPost]
        [Route("saveAcademicYear")]
        public async Task<IActionResult> SaveAcademicYear([FromBody] AcademicRequest academicRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new AcademicSaveCommand(academicRequest.id, academicRequest.yearAcademic, StringToDate(academicRequest.fromDate), StringToDate(academicRequest.toDate), academicRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpGet]
        [Route("getAcademicTerm")]
        public async Task<JsonResult> GetAcademicTerms()
        {
            var userId = GetUserIdFromToken();

            var academicTermEnvelop = await mediator.Send((IRequest<AcademicTermEnvelop>)new AcademicTermQuery(userId));

            var response = AcademicTermResponses.PopulateAcademicTermResponses(academicTermEnvelop.academicTerms);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpPost]
        [Route("saveAcademicTerm")]
        public async Task<IActionResult> SaveAcademicTerm([FromBody] AcademicTermRequest academicTermRequest)
        {
            var userId = GetUserIdFromToken();
            
            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new AcademicTermSaveCommand(academicTermRequest.id, academicTermRequest.term, academicTermRequest.yearAcademic, StringToDate(academicTermRequest.fromDate), StringToDate(academicTermRequest.toDate), academicTermRequest.isActive, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpGet]
        [Route("getClasses")]
        public async Task<JsonResult> GetClasses()
        {
            var userId = GetUserIdFromToken();

            var academicClassEnvelop = await mediator.Send((IRequest<AcademicClassEnvelop>)new AcademicClassQuery(userId));

            var response = AcademicClassResponses.PopulateAcademicClassResponses(academicClassEnvelop.academicClasses);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
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

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpGet]
        [Route("getSubject")]
        public async Task<JsonResult> GetSubject()
        {
            var userId = GetUserIdFromToken();

            var subjectEnvelop = await mediator.Send((IRequest<SubjectEnvelop>)new SubjectQuery(userId));

            var response = AcademicSubjectResponses.PopulateAcademicSubjectResponses(subjectEnvelop.subjects);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
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

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpGet]
        [Route("getStudentEnroll")]
        public async Task<JsonResult> GetStudentEnroll(long classId, long academicYear)
        {
            var userId = GetUserIdFromToken();

            var studentEnrollEnvelop = await mediator.Send((IRequest<StudentEnrollEnvelop>)new StudentEnrollQuery(classId, academicYear, userId));

            var response = StudentEnrollResponses.PopulateStudentEnrollResponses(studentEnrollEnvelop.studentEnrolls);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
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

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpGet]
        [Route("getClassSubject")]
        public async Task<JsonResult> GetClassSubject(long classId)
        {
            var userId = GetUserIdFromToken();

            var studentEnrollEnvelop = await mediator.Send((IRequest<AcademicClassSubjectEnvelop>)new ClassSubjectQuery(classId, userId));

            var response = AcademicClassSubjectResponses.PopulateAcademicClassSubjectResponses(studentEnrollEnvelop.academicClassSubjects);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
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

        [Authorize(Roles = "Staff,StaffAdministrator")]
        [HttpGet]
        [Route("getSubjectChapter")]
        public async Task<JsonResult> GetSubjectChapter(long subjectId)
        {
            var userId = GetUserIdFromToken();

            var subjectChapterEnvelop = await mediator.Send((IRequest<AcademicSubjectChapterEnvelop>)new SubjectChapterQuery(subjectId, userId));

            var response = AssociateSubjectChapterResponses.PopulateAssociateSubjectChapterResponses(subjectChapterEnvelop.subjectChapters);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Staff,StaffAdministrator")]
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

        [Authorize(Roles = "Staff,StaffAdministrator")]
        [HttpGet]
        [Route("getChapterTopic")]
        public async Task<JsonResult> GetChapterTopic(long chapterId)
        {
            var userId = GetUserIdFromToken();

            var chapterTopicEnvelop = await mediator.Send((IRequest<AcademicChapterTopicEnvelop>)new ChapterTopicQuery(chapterId, userId));

            var response = AssociateChapterTopicResponses.PopulateAssociateChapterTopicResponses(chapterTopicEnvelop.chapterTopics);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Staff,StaffAdministrator")]
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

        [Authorize(Roles = "Staff,StaffAdministrator")]
        [HttpGet]
        [Route("getTopicContent")]
        public async Task<JsonResult> GetTopicContent(long topicId)
        {
            var userId = GetUserIdFromToken();

            var topicContentEnvelop = await mediator.Send((IRequest<AcademicTopicContentEnvelop>)new TopicContentQuery(topicId, userId));

            var response = AssociateTopicContentResponses.PopulateAssociateTopicContentResponses(topicContentEnvelop.topicContents);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Staff,StaffAdministrator")]
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

        [Authorize(Roles = "Staff,StaffAdministrator")]
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

        [Authorize(Roles = "Staff,StaffAdministrator")]
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

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpGet]
        [Route("getAllTimeTable")]
        public async Task<JsonResult> GetAllTimeTable(long classId)
        {
            var userId = GetUserIdFromToken();

            var timeTableEnvelop = await mediator.Send((IRequest<TimeTableEnvelop>)new AllTimeTableQuery(classId, userId));

            var response = TimeTableResponses.PopulateTimeTableResponses(timeTableEnvelop.weekDays);

            return new JsonResult(response);
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpPost]
        [Route("saveTimeTable")]
        public async Task<IActionResult> SaveTimeTable([FromBody] TimeTableRequest timeTableRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new TimeTableSaveCommand(timeTableRequest.id, timeTableRequest.classId, timeTableRequest.subjectId, timeTableRequest.fromTime, timeTableRequest.toTime, timeTableRequest.weekDayId, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }

        [Authorize(Roles = "Administrator,StaffAdministrator")]
        [HttpPost]
        [Route("removeTimeSlot")]
        public async Task<IActionResult> RemoveTimeSlot([FromBody] RemoveTimeTableRequest removeTimeTableRequest)
        {
            var userId = GetUserIdFromToken();

            var result = await mediator.Send((IRequest<RequestSaveEnvelop>)new RemoveTimeTableSaveCommand(removeTimeTableRequest.id, userId));

            if (result.Created)
            {
                return Ok(new JsonResult(result));
            }

            return BadRequest(new JsonResult(result));
        }
    }
}
