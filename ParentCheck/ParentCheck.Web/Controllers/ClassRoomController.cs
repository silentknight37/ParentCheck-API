using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Web.Common;
using ParentCheck.Web.Common.Responses;
using ParentCheck.Web.Helpers;
using ParentCheck.Web.Models;
using System;
using System.Threading.Tasks;

namespace ParentCheck.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassRoomController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly JwtService jwtservice;

        public ClassRoomController(IMediator mediator, JwtService jwtservice)
        {
            this.mediator = mediator;
            this.jwtservice = jwtservice;
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
    }
}
