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
                                                   s.Subject,
                                                   s.DescriptionText,
                                                   cs.BgColor,
                                                   cs.FontColor
                                               }).ToListAsync();

                foreach (var userClassSubject in userClassSubjects)
                {
                    userClass.Subjects.Add(new UserSubjectDTO
                    {
                        InstituteClassSubjectId = userClassSubject.Id,
                        Subject = userClassSubject.Subject,
                        Description = userClassSubject.DescriptionText,
                        ColorBg = userClassSubject.BgColor,
                        ColorFont= userClassSubject.FontColor
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
                                   s.Subject,
                                   cs.BgColor,
                                   cs.FontColor
                               }).FirstOrDefault();

            if (userSubject != null) {
                subjectChapter.Subject = userSubject.Subject;
                subjectChapter.ColorBg = userSubject.BgColor;
                subjectChapter.ColorFont = userSubject.FontColor;

                var userSubjectChapters = await (from sc in _parentcheckContext.InstituteSubjectChapter
                                                 where sc.InstituteClassSubjectId == classSubjectId
                                                 select new
                                                 {
                                                     sc.Id,
                                                     sc.Chapter,
                                                     sc.InstituteAssignmentId
                                                 }).ToListAsync();

                foreach (var userSubjectChapter in userSubjectChapters)
                {
                    var chapterTopicCount = await (from ct in _parentcheckContext.InstituteChapterTopic
                                                   where ct.InstituteSubjectChapterId == userSubjectChapter.Id
                                                   select new
                                                   {
                                                       ct.Id
                                                   }).ToListAsync();

                    subjectChapter.Chapters.Add(new SubjectChapterDTO
                    {
                        InstituteSubjectChapterId = userSubjectChapter.Id,
                        Chapter = userSubjectChapter.Chapter,
                        TopicCount = chapterTopicCount.Count
                    });
                }

            }

            return subjectChapter;
        }

        public async Task<UserChapterTopicsDTO> GetUserChaptersTopicsAsync(long subjectChapterId, long userId)
        {
            UserChapterTopicsDTO chapterTopic = new UserChapterTopicsDTO();

            var userChapter = (from sc in _parentcheckContext.InstituteSubjectChapter
                               join cs in _parentcheckContext.InstituteClassSubject on sc.InstituteClassSubjectId equals cs.Id
                               where sc.Id == subjectChapterId
                               select new
                               {
                                   sc.Id,
                                   sc.Chapter,
                                   cs.BgColor,
                                   cs.FontColor,
                                   sc.InstituteAssignmentId
                               }).FirstOrDefault();

            if (userChapter != null)
            {
                chapterTopic.Chapter = userChapter.Chapter;
                chapterTopic.ColorBg = userChapter.BgColor;
                chapterTopic.ColorFont = userChapter.FontColor;

                chapterTopic.IsAssignmentAssign = false;
                if (userChapter.InstituteAssignmentId.HasValue)
                {
                    var userAssignments = await _parentcheckContext.InstituteAssignment.Where(i => i.Id == userChapter.InstituteAssignmentId.Value).FirstOrDefaultAsync();

                    if (userAssignments != null)
                    {
                        chapterTopic.IsAssignmentAssign = true;
                        chapterTopic.Assignment = new AssignmentDTO
                        {
                            Id = userAssignments.Id,
                            AssignmentDescription = userAssignments.AssignmentDescription,
                            AssignmentTitle = userAssignments.AssignmentTitle,
                            OpenDate = userAssignments.OpenDate,
                            CloseDate = userAssignments.CloseDate,
                            AssignmentDocuments = await GetAssignmentDocuments(userAssignments.Id)
                        };
                    }
                }


                var userChapterTopics = await (from ct in _parentcheckContext.InstituteChapterTopic
                                                 where ct.InstituteSubjectChapterId == subjectChapterId
                                                 select new
                                                 {
                                                     ct.Id,
                                                     ct.Topic,
                                                     ct.DescriptionText,
                                                     ct.CreatedOn,
                                                     ct.InstituteAssignmentId
                                                 }).ToListAsync();

                foreach (var userChapterTopic in userChapterTopics)
                {
                    chapterTopic.ChapterTopics.Add(new ChapterTopicsDTO
                    {
                        InstituteChapterTopicId = userChapterTopic.Id,
                        Topic = userChapterTopic.Topic,
                        Description= userChapterTopic.DescriptionText,
                        SubmitDate = userChapterTopic.CreatedOn.Value.ToShortDateString()
                    });
                }

            }

            return chapterTopic;
        }

        public async Task<UserTopicContentsDTO> GetUserTopicContentAsync(long chapterTopicId, long userId)
        {
            UserTopicContentsDTO topicContents = new UserTopicContentsDTO();

            var userTopic = (from ct in _parentcheckContext.InstituteChapterTopic
                             join sc in _parentcheckContext.InstituteSubjectChapter on ct.InstituteSubjectChapterId equals sc.Id
                             join cs in _parentcheckContext.InstituteClassSubject on sc.InstituteClassSubjectId equals cs.Id
                               where ct.Id == chapterTopicId
                               select new
                               {
                                   ct.Id,
                                   ct.Topic,
                                   cs.BgColor,
                                   cs.FontColor,
                                   ct.InstituteAssignmentId,
                                   cs.InstituteSubjectId
                               }).FirstOrDefault();

            if (userTopic != null)
            {

                topicContents.Topic = userTopic.Topic;
                topicContents.ColorBg = userTopic.BgColor;
                topicContents.ColorFont = userTopic.FontColor;
                topicContents.SubjectId = userTopic.InstituteSubjectId;

                topicContents.IsAssignmentAssign = false;
                if (userTopic.InstituteAssignmentId.HasValue)
                {
                    var userAssignments = await _parentcheckContext.InstituteAssignment.Where(i => i.Id == userTopic.InstituteAssignmentId.Value).FirstOrDefaultAsync();

                    if (userAssignments != null)
                    {
                        topicContents.IsAssignmentAssign = true;
                        topicContents.Assignment = new AssignmentDTO
                        {
                            Id = userAssignments.Id,
                            AssignmentDescription = userAssignments.AssignmentDescription,
                            AssignmentTitle = userAssignments.AssignmentTitle,
                            OpenDate = userAssignments.OpenDate,
                            CloseDate = userAssignments.CloseDate,
                            AssignmentDocuments = await GetAssignmentDocuments(userAssignments.Id)
                        };
                    }
                }                

                var userTopicContents = await (from tc in _parentcheckContext.InstituteTopicContent
                                               join ct in _parentcheckContext.ContentType on tc.ContentTypeId equals ct.Id
                                               where tc.InstituteChapterTopicId == chapterTopicId
                                               select new
                                               {
                                                   tc.Id,
                                                   tc.ContentTypeId,
                                                   ct.TypeText,
                                                   tc.ContentText,
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
                        ContentOrder= userTopicContent.ContentOrder,
                        ContentDocuments=await GetContentDocuments(userTopicContent.Id)
                    });


                }

            }

            return topicContents;
        }

        private async Task<List<ContentDocumentDTO>>  GetContentDocuments(long topicContentId)
        {
            List<ContentDocumentDTO> contentDocuments = new List<ContentDocumentDTO>();

            var contentDocs = await _parentcheckContext.InstituteTopicContentDocument.Where(i => i.InstituteTopicContentId == topicContentId && i.IsActive==true).ToListAsync();

            contentDocs.ForEach(i => contentDocuments.Add(new ContentDocumentDTO
            {
                Id=i.Id,
                InstituteTopicContentId=i.InstituteTopicContentId,
                FileName=i.FileName,
                Url=i.ContentUrl
            }));


            return contentDocuments;
        }

        private async Task<List<AssignmentDocumentDTO>>  GetAssignmentDocuments(long assignmentId)
        {
            List<AssignmentDocumentDTO> assignmentDocument = new List<AssignmentDocumentDTO>();

            var contentDocs = await _parentcheckContext.InstituteAssignmentDocument.Where(i => i.InstituteAssignmentId == assignmentId && i.IsActive==true).ToListAsync();

            contentDocs.ForEach(i => assignmentDocument.Add(new AssignmentDocumentDTO
            {
                Id=i.Id,
                InstituteAssignmentId = i.InstituteAssignmentId,
                FileName=i.FileName,
                Url=i.ContentUrl,
                AssignmentTypeId=i.ContentTypeId
            }));


            return assignmentDocument;
        }
    }
}
