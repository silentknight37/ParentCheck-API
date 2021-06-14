using Microsoft.EntityFrameworkCore;
using ParentCheck.BusinessObject;
using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository
{
    public class ClassRoomRepository : IClassRoomRepository
    {
        private ParentCheckContext _parentcheckContext;

        public ClassRoomRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }

        public async Task<UserClassDTO> GetUserSubjectAsync(long userId)
        {
            UserClassDTO userClass = new UserClassDTO();

            var userActiveClass = await (from cu in _parentcheckContext.InstituteUserClass
                                         join c in _parentcheckContext.InstituteClass on cu.InstituteClassId equals c.Id
                                         join ay in _parentcheckContext.AcademicYear on cu.AcademicYearId equals ay.Id
                                         where cu.InstituteUserId == userId && ay.FromDate <= DateTime.UtcNow && ay.ToDate >= DateTime.UtcNow
                                         select new
                                         {
                                             cu.InstituteClassId,
                                             c.Class
                                         }).FirstOrDefaultAsync();

            if (userActiveClass != null)
            {
                userClass.UserClass = userActiveClass.Class;

                var userClassSubjects = await (from cs in _parentcheckContext.InstituteClassSubject
                                               join s in _parentcheckContext.InstituteSubject on cs.InstituteSubjectId equals s.Id
                                               where cs.InstituteClassId == userActiveClass.InstituteClassId
                                               select new
                                               {
                                                   cs.Id,
                                                   s.Subject
                                               }).ToListAsync();

                foreach (var userClassSubject in userClassSubjects)
                {
                    userClass.Subjects.Add(new UserSubjectDTO
                    {
                        InstituteClassSubjectId = userClassSubject.Id,
                        Subject = userClassSubject.Subject
                    });
                }
            }

            return userClass;
        }

        public async Task<UserSubjectChapterDTO> GetUserSubjectChaptersAsync(long classSubjectId, long userId)
        {
            UserSubjectChapterDTO subjectChapter = new UserSubjectChapterDTO();

            var userSubject = (from cs in _parentcheckContext.InstituteClassSubject
                               join s in _parentcheckContext.InstituteSubject on cs.InstituteSubjectId equals s.Id
                               where cs.Id == classSubjectId
                               select new
                               {
                                   cs.Id,
                                   s.Subject
                               }).FirstOrDefault();

            if (userSubject != null) {
                subjectChapter.Subject = userSubject.Subject;

                var userSubjectChapters = await (from sc in _parentcheckContext.InstituteSubjectChapter
                                                 where sc.InstituteClassSubjectId == classSubjectId
                                                 select new
                                                 {
                                                     sc.Id,
                                                     sc.Chapter
                                                 }).ToListAsync();

                foreach (var userSubjectChapter in userSubjectChapters)
                {
                    subjectChapter.Chapters.Add(new SubjectChapterDTO
                    {
                        InstituteSubjectChapterId = userSubjectChapter.Id,
                        Chapter = userSubjectChapter.Chapter
                    });
                }

            }

            return subjectChapter;
        }

        public async Task<UserChapterTopicsDTO> GetUserChaptersTopicsAsync(long subjectChapterId, long userId)
        {
            UserChapterTopicsDTO chapterTopic = new UserChapterTopicsDTO();

            var userChapter = (from sc in _parentcheckContext.InstituteSubjectChapter
                               where sc.Id == subjectChapterId
                               select new
                               {
                                   sc.Id,
                                   sc.Chapter
                               }).FirstOrDefault();

            if (userChapter != null)
            {
                chapterTopic.Chapter = userChapter.Chapter;

                var userChapterTopics = await (from ct in _parentcheckContext.InstituteChapterTopic
                                                 where ct.InstituteSubjectChapterId == subjectChapterId
                                                 select new
                                                 {
                                                     ct.Id,
                                                     ct.Topic,
                                                     ct.DescriptionText
                                                 }).ToListAsync();

                foreach (var userChapterTopic in userChapterTopics)
                {
                    chapterTopic.ChapterTopics.Add(new ChapterTopicsDTO
                    {
                        InstituteChapterTopicId = userChapterTopic.Id,
                        Topic = userChapterTopic.Topic,
                        Description= userChapterTopic.DescriptionText
                    });
                }

            }

            return chapterTopic;
        }


        public async Task<UserTopicContentsDTO> GetUserTopicContentAsync(long chapterTopicId, long userId)
        {
            UserTopicContentsDTO topicContents = new UserTopicContentsDTO();

            var userTopic = (from ct in _parentcheckContext.InstituteChapterTopic
                               where ct.Id == chapterTopicId
                               select new
                               {
                                   ct.Id,
                                   ct.Topic
                               }).FirstOrDefault();

            if (userTopic != null)
            {
                topicContents.Topic = userTopic.Topic;

                var userTopicContents = await (from tc in _parentcheckContext.InstituteTopicContent
                                               join ct in _parentcheckContext.ContentType on tc.ContentTypeId equals ct.Id
                                               where tc.InstituteChapterTopicId == chapterTopicId
                                               select new
                                               {
                                                   tc.Id,
                                                   tc.ContentTypeId,
                                                   ct.TypeText,
                                                   tc.ContentText,
                                                   tc.ContentUrl,
                                                   tc.ContentOrder
                                               }).ToListAsync();

                foreach (var userTopicContent in userTopicContents)
                {
                    topicContents.TopicContents.Add(new TopicContentDTO
                    {
                        InstituteTopicContentId= userTopicContent.Id,
                        ContentText= userTopicContent.ContentText,
                        ContentType= userTopicContent.TypeText,
                        ContentTypeId= userTopicContent.ContentTypeId,
                        ContentURL= userTopicContent.ContentUrl,
                        ContentOrder= userTopicContent.ContentOrder
                    });
                }

            }

            return topicContents;
        }
    }
}
