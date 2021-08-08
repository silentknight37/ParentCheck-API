using Microsoft.EntityFrameworkCore;
using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                        ColorFont = userClassSubject.FontColor
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

            if (userSubject != null)
            {
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
                        Description = userChapterTopic.DescriptionText,
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
                        InstituteTopicContentId = userTopicContent.Id,
                        ContentText = userTopicContent.ContentText,
                        ContentType = userTopicContent.TypeText,
                        ContentTypeId = userTopicContent.ContentTypeId,
                        ContentOrder = userTopicContent.ContentOrder,
                        ContentDocuments = await GetContentDocuments(userTopicContent.Id)
                    });


                }

            }

            return topicContents;
        }

        private async Task<List<ContentDocumentDTO>> GetContentDocuments(long topicContentId)
        {
            List<ContentDocumentDTO> contentDocuments = new List<ContentDocumentDTO>();

            var contentDocs = await _parentcheckContext.InstituteTopicContentDocument.Where(i => i.InstituteTopicContentId == topicContentId && i.IsActive == true).ToListAsync();

            contentDocs.ForEach(i => contentDocuments.Add(new ContentDocumentDTO
            {
                Id = i.Id,
                InstituteTopicContentId = i.InstituteTopicContentId,
                FileName = i.FileName,
                Url = i.ContentUrl
            }));


            return contentDocuments;
        }

        private async Task<List<AssignmentDocumentDTO>> GetAssignmentDocuments(long assignmentId)
        {
            List<AssignmentDocumentDTO> assignmentDocument = new List<AssignmentDocumentDTO>();

            var contentDocs = await _parentcheckContext.InstituteAssignmentDocument.Where(i => i.InstituteAssignmentId == assignmentId && i.IsActive == true).ToListAsync();

            contentDocs.ForEach(i => assignmentDocument.Add(new AssignmentDocumentDTO
            {
                Id = i.Id,
                InstituteAssignmentId = i.InstituteAssignmentId,
                FileName = i.FileName,
                Url = i.ContentUrl,
                AssignmentTypeId = i.ContentTypeId
            }));


            return assignmentDocument;
        }

        public async Task<UserSubmitedAssignmentFileDTO> GetSubmitedAssignmentFileAsync(long assignmentId, long userId)
        {
            UserSubmitedAssignmentFileDTO userSubmitedAssignmentFile = new UserSubmitedAssignmentFileDTO();

            var assignmentSubmission = await (from ias in _parentcheckContext.InstituteAssignmentSubmission
                                              join s in _parentcheckContext.Status on ias.StatusId equals s.Id
                                              where ias.InstituteAssignmentId == assignmentId && ias.SubmitUserId == userId
                                              select new
                                              {
                                                  ias.Id,
                                                  ias.CompleteDate,
                                                  ias.StatusId,
                                                  s.StatusText
                                              }).FirstOrDefaultAsync();

            if (assignmentSubmission != null)
            {
                userSubmitedAssignmentFile.AssignmentSubmissionId = assignmentSubmission.Id;
                userSubmitedAssignmentFile.CompleteDate = assignmentSubmission.CompleteDate;
                userSubmitedAssignmentFile.StatusId = assignmentSubmission.StatusId;
                userSubmitedAssignmentFile.StatusText = assignmentSubmission.StatusText;

                var assignmentSubmissionDocuments = await (from iasd in _parentcheckContext.InstituteAssignmentSubmissionDocument
                                                           where iasd.AssignmentSubmissionId == assignmentSubmission.Id
                                                           select new
                                                           {
                                                               iasd.AssignmentSubmissionId,
                                                               iasd.Id,
                                                               iasd.FileName,
                                                               iasd.EncryptedFileName,
                                                               iasd.ContentUrl,
                                                               iasd.ContentTypeId
                                                           }).ToListAsync();

                foreach (var submissionDocuments in assignmentSubmissionDocuments)
                {
                    userSubmitedAssignmentFile.AssignmentSubmissionDocuments.Add(new AssignmentSubmissionDocumentDTO
                    {
                        AssignmentSubmissionId = submissionDocuments.AssignmentSubmissionId,
                        InstituteAssignmentSubmissionDocumentId = submissionDocuments.Id,
                        FileName = submissionDocuments.FileName,
                        EncryptedFileName = submissionDocuments.EncryptedFileName,
                        Url = submissionDocuments.ContentUrl,
                        DocumentTypeId = submissionDocuments.ContentTypeId
                    });
                }
            }

            return userSubmitedAssignmentFile;
        }

        public async Task<bool> UploadAssignmentFileAsync(long assignmentId, string encryptedFileName, string uploadPath, string fileName, long userId)
        {
            var user = _parentcheckContext.User.Where(i => i.Id == userId && i.IsActive == true).FirstOrDefault();

            if (user != null)
            {
                var assignment = await _parentcheckContext.InstituteAssignmentSubmission.FirstOrDefaultAsync(i => i.InstituteAssignmentId == assignmentId);
                if (assignment == null)
                {
                    assignment = new InstituteAssignmentSubmission();
                    assignment.InstituteAssignmentId = assignmentId;
                    assignment.SubmitUserId = userId;
                    assignment.StatusId = (int)EnumStatus.PartiallyCompleted;

                    assignment.CreatedOn = DateTime.UtcNow;
                    assignment.CreatedBy = $"{user.FirstName} {user.LastName}";
                    assignment.UpdateOn = DateTime.UtcNow;
                    assignment.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteAssignmentSubmission.Add(assignment);
                    try
                    {
                        await _parentcheckContext.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {

                        throw;
                    }

                }

                InstituteAssignmentSubmissionDocument submissionDocument = new InstituteAssignmentSubmissionDocument();
                submissionDocument.FileName = fileName;
                submissionDocument.EncryptedFileName = encryptedFileName;
                submissionDocument.ContentUrl = $"http://storage.parentcheck.lk/{uploadPath}/{encryptedFileName}";
                submissionDocument.AssignmentSubmissionId = assignment.Id;
                submissionDocument.ContentTypeId = 1;

                submissionDocument.IsActive = true;
                submissionDocument.CreatedOn = DateTime.UtcNow;
                submissionDocument.CreatedBy = $"{user.FirstName} {user.LastName}";
                submissionDocument.UpdateOn = DateTime.UtcNow;
                submissionDocument.UpdatedBy = $"{user.FirstName} {user.LastName}";

                _parentcheckContext.InstituteAssignmentSubmissionDocument.Add(submissionDocument);
                try
                {
                    await _parentcheckContext.SaveChangesAsync();
                }
                catch (Exception e)
                {

                    throw;
                }
                return true;
            }

            return false;
        }

        public async Task<bool> UploadLibrayFileAsync(long instituteId, string libraryDescription, string encryptedFileName, string uploadPath, string fileName, bool isInstituteLevel, int contentType, long userId)
        {
            var user = _parentcheckContext.User.Where(i => i.Id == userId && i.IsActive == true).FirstOrDefault();

            if (user != null)
            {
                InstituteLibrary instituteLibrary = new InstituteLibrary();
                instituteLibrary.FileName = fileName;
                instituteLibrary.InstituteId = instituteId;
                instituteLibrary.EncryptedFileName = encryptedFileName;
                instituteLibrary.LibraryDescription = libraryDescription;
                instituteLibrary.ContentUrl = $"http://storage.parentcheck.lk/{uploadPath}/{encryptedFileName}";
                instituteLibrary.ContentTypeId = contentType;

                instituteLibrary.IsActive = true;
                instituteLibrary.IsInstituteLevelAccess = isInstituteLevel;
                instituteLibrary.IsGlobal = !isInstituteLevel;
                instituteLibrary.CreatedOn = DateTime.UtcNow;
                instituteLibrary.CreatedBy = $"{user.FirstName} {user.LastName}";
                instituteLibrary.UpdateOn = DateTime.UtcNow;
                instituteLibrary.UpdatedBy = $"{user.FirstName} {user.LastName}";

                _parentcheckContext.InstituteLibrary.Add(instituteLibrary);
                try
                {
                    await _parentcheckContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw;
                }
                return true;
            }

            return false;
        }

        public async Task<long> RemoveAssignmentFileAsync(long submissionId, long id)
        {
            var assignmentSubmittion = _parentcheckContext.InstituteAssignmentSubmission.Where(i => i.Id == submissionId).FirstOrDefault();

            var assignmentFile = _parentcheckContext.InstituteAssignmentSubmissionDocument.Where(i => i.Id == id).FirstOrDefault();
            if (assignmentFile != null)
            {
                _parentcheckContext.InstituteAssignmentSubmissionDocument.Remove(assignmentFile);
                await _parentcheckContext.SaveChangesAsync();

                return assignmentSubmittion.InstituteAssignmentId;
            }
            return 0;
        }

        public async Task<bool> CompleteAssignment(long assignmentId, long userId)
        {
            var assignmentSubmittion = _parentcheckContext.InstituteAssignmentSubmission.FirstOrDefault(i => i.InstituteAssignmentId == assignmentId && i.SubmitUserId == userId);
            if (assignmentSubmittion != null)
            {
                assignmentSubmittion.StatusId = (int)EnumStatus.Completed;
                assignmentSubmittion.CompleteDate = DateTime.UtcNow;
                assignmentSubmittion.UpdatedBy = userId.ToString();
                assignmentSubmittion.UpdateOn = DateTime.UtcNow;
                _parentcheckContext.Entry(assignmentSubmittion).State = EntityState.Modified;
                await _parentcheckContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public async Task<List<ClassRoomOverviewDTO>> GetClassRoomOverviewAsync(bool isToday, bool isThisWeek, bool isNextWeek, bool isCustom, DateTime? fromDate, DateTime? toDate, long? subjectId, long? instituteTermsId, long userId)
        {
            List<ClassRoomOverviewDTO> classRoomOverviews = new List<ClassRoomOverviewDTO>();

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
                var topicContents = await (from tc in _parentcheckContext.InstituteTopicContent
                                           join ct in _parentcheckContext.InstituteChapterTopic on tc.InstituteChapterTopicId equals ct.Id
                                           join sc in _parentcheckContext.InstituteSubjectChapter on ct.InstituteSubjectChapterId equals sc.Id
                                           join cs in _parentcheckContext.InstituteClassSubject on sc.InstituteClassSubjectId equals cs.Id
                                           join s in _parentcheckContext.InstituteSubject on cs.InstituteSubjectId equals s.Id
                                           where (subjectId == 0 || sc.InstituteClassSubjectId == subjectId.Value) &&
                                           cs.InstituteClassId == userActiveClass.InstituteClassId
                                           select new
                                           {
                                               tc.CreatedOn,
                                               s.Subject,
                                               sc.Chapter,
                                               ct.Topic,
                                               subjectChapterId = sc.Id,
                                               topicId = ct.Id
                                           }).ToListAsync();

                var termChapters = await (from tc in _parentcheckContext.InstituteTermChapter
                                          join t in _parentcheckContext.InstituteTerm on tc.InstituteTermId equals t.Id
                                          where (instituteTermsId == 0 || tc.InstituteTermId == instituteTermsId.Value)
                                          select new
                                          {
                                              tc.InstituteSubjectChapterId,
                                              t.Term
                                          }).ToListAsync();
                if (isToday)
                {
                    topicContents = topicContents.Where(i => i.CreatedOn.Value.ToShortDateString() == DateTime.Now.ToShortDateString()).ToList();
                }

                if (isNextWeek)
                {
                    topicContents = topicContents.Where(i => (GetIso8601WeekOfYear(i.CreatedOn.Value)) == (GetIso8601WeekOfYear(DateTime.Now.AddDays(7)))).ToList();
                }

                if (isThisWeek)
                {
                    topicContents = topicContents.Where(i => GetIso8601WeekOfYear(i.CreatedOn.Value) == GetIso8601WeekOfYear(DateTime.Now)).ToList();
                }

                if (isCustom)
                {
                    topicContents = topicContents.Where(i => (fromDate == null || i.CreatedOn.Value >= fromDate.Value.Date) && (toDate == null || i.CreatedOn <= toDate.Value.Date)).ToList();
                }


                foreach (var topicContent in topicContents)
                {
                    var filterTerm = termChapters.Where(i => i.InstituteSubjectChapterId == topicContent.subjectChapterId).FirstOrDefault();

                    if (filterTerm == null)
                    {
                        continue;
                    }

                    classRoomOverviews.Add(new ClassRoomOverviewDTO
                    {
                        TopicId = topicContent.topicId,
                        Subject = topicContent.Subject,
                        Chapter = topicContent.Chapter,
                        Topic = topicContent.Topic,
                        Date = topicContent.CreatedOn.Value,
                        Term = filterTerm.Term
                    });
                }
            }

            return classRoomOverviews;
        }


        public async Task<List<LibraryDTO>> GetLibraryAsync(long userId)
        {
            List<LibraryDTO> librarieFiles = new List<LibraryDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {

                var globalFiles = await (from l in _parentcheckContext.InstituteLibrary
                                         where l.IsGlobal == true
                                         select l).ToListAsync();

                foreach (var globalFile in globalFiles)
                {
                    librarieFiles.Add(new LibraryDTO
                    {
                        Id = globalFile.Id,
                        FileName = globalFile.FileName,
                        ContentURL = globalFile.ContentUrl,
                        ContentTypeId = globalFile.ContentTypeId,
                        IsGlobal = globalFile.IsGlobal,
                        LibraryDescription = globalFile.LibraryDescription
                    });
                }

                var instituteGlobalFiles = await (from l in _parentcheckContext.InstituteLibrary
                                                  where l.IsInstituteLevelAccess == true && l.InstituteId == user.InstituteId
                                                  select l).ToListAsync();

                foreach (var instituteGlobalFile in instituteGlobalFiles)
                {
                    librarieFiles.Add(new LibraryDTO
                    {
                        Id = instituteGlobalFile.Id,
                        FileName = instituteGlobalFile.FileName,
                        ContentURL = instituteGlobalFile.ContentUrl,
                        ContentTypeId = instituteGlobalFile.ContentTypeId,
                        IsInstituteLevelAccess = instituteGlobalFile.IsInstituteLevelAccess,
                        LibraryDescription = instituteGlobalFile.LibraryDescription,
                        InstituteId = instituteGlobalFile.InstituteId
                    });
                }

                var classLibrarieFiles = await (from l in _parentcheckContext.InstituteLibrary
                                                where l.IsGlobal == false &&
                                                l.IsInstituteLevelAccess == false &&
                                                l.InstituteId == user.InstituteId
                                                select l).ToListAsync();

                foreach (var classLibrarieFile in classLibrarieFiles)
                {
                    librarieFiles.Add(new LibraryDTO
                    {
                        Id = classLibrarieFile.Id,
                        FileName = classLibrarieFile.FileName,
                        ContentURL = classLibrarieFile.ContentUrl,
                        ContentTypeId = classLibrarieFile.ContentTypeId,
                        LibraryDescription = classLibrarieFile.LibraryDescription,
                        InstituteId = classLibrarieFile.InstituteId
                    });
                }

            }

            return librarieFiles;
        }

        private int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public async Task<List<ClassStudentAttendancesDTO>> GetClassStudentAttendancesAsync(long classId, DateTime recordDate, long userId)
        {
            List<ClassStudentAttendancesDTO> ClassStudentAttendancesDTO = new List<ClassStudentAttendancesDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var classStudents = await (from ic in _parentcheckContext.InstituteUserClass
                                           join a in _parentcheckContext.AcademicYear on ic.AcademicYearId equals a.Id
                                           join iu in _parentcheckContext.InstituteUser on ic.InstituteUserId equals iu.Id
                                           join c in _parentcheckContext.InstituteClass on ic.InstituteClassId equals c.Id
                                           where ic.InstituteClassId == classId && a.FromDate.Date <= DateTime.Now.Date && a.ToDate.Date >= DateTime.Now.Date
                                           select new
                                           {
                                               c.Id,
                                               ic.InstituteUserId,
                                               iu.FirstName,
                                               iu.LastName,
                                               iu.InstituteId,
                                               c.Class
                                           }).ToListAsync();

                foreach (var classStudent in classStudents)
                {
                    var studentAttendance = await (from sa in _parentcheckContext.StudentAttendance
                                                    where sa.ResponsibleUserId == user.Id 
                                                    && sa.InstituteClassId == classId 
                                                    && sa.RecordDate.Date == recordDate.Date
                                                    && sa.InstituteUserId== classStudent.InstituteUserId
                                                    select new
                                                    {
                                                        sa.Id,
                                                        sa.InstituteId,
                                                        sa.InstituteUserClassId,
                                                        sa.RecordDate,
                                                        sa.IsAttendance,
                                                        sa.InstituteUserId,
                                                        sa.ResponsibleUserId
                                                    }).FirstOrDefaultAsync();

                    ClassStudentAttendancesDTO.Add(new ClassStudentAttendancesDTO
                    {
                        Id = studentAttendance!=null?studentAttendance.Id:0,
                        InstituteId = classStudent.InstituteId,
                        InstituteClassId= classStudent.Id,
                        InstituteUserId = classStudent.InstituteUserId,
                        IsAttendance = studentAttendance!=null?studentAttendance.IsAttendance:false,
                        IsMarked = studentAttendance!=null?true:false,
                        RecordDate = recordDate,
                        ResponsibleUserFirstName = user.FirstName,
                        ResponsibleUserLastName = user.LastName,
                        UserFirstName = classStudent.FirstName,
                        UserLastName = classStudent.LastName,
                        UserClassName = classStudent.Class
                    });
                }
            }

            return ClassStudentAttendancesDTO;
        }

        public async Task<List<ClassStudentDTO>> GetClassStudentAsync(long classId, long userId)
        {
            List<ClassStudentDTO> classStudentDTOs = new List<ClassStudentDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var classStudents = await (from ic in _parentcheckContext.InstituteUserClass
                                           join a in _parentcheckContext.AcademicYear on ic.AcademicYearId equals a.Id
                                           join iu in _parentcheckContext.InstituteUser on ic.InstituteUserId equals iu.Id
                                           where ic.InstituteUserId == user.Id && ic.InstituteClassId == classId && a.FromDate.Date<=DateTime.Now.Date && a.ToDate.Date>=DateTime.Now.Date
                                           select new
                                           {
                                               ic.Id,
                                               ic.InstituteUserId,
                                               iu.FirstName,
                                               iu.LastName
                                           }).ToListAsync();

                foreach (var classStudent in classStudents)
                {
                    classStudentDTOs.Add(new ClassStudentDTO
                    {
                        Id = classStudent.Id,
                        InstituteUserId = classStudent.InstituteUserId,
                        UserFirstName = classStudent.FirstName,
                        UserLastName = classStudent.LastName
                    });
                }
            }

            return classStudentDTOs;
        }

        public async Task<bool> SaveClassStudentAttendanceAsync(long instituteUserId, long instituteClassId, DateTime recordDate, bool isAttendance, long userId)
        {
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var userClass = _parentcheckContext.InstituteUserClass.FirstOrDefault(i => i.InstituteUserId == instituteUserId && i.InstituteClassId == instituteClassId);

                if (userClass != null)
                {
                    StudentAttendance studentAttendance = new StudentAttendance();
                    studentAttendance.InstituteUserId = instituteUserId;
                    studentAttendance.InstituteClassId = instituteClassId;
                    studentAttendance.InstituteId = user.InstituteId;
                    studentAttendance.RecordDate = recordDate;
                    studentAttendance.IsAttendance = isAttendance;
                    studentAttendance.ResponsibleUserId = user.Id;
                    studentAttendance.InstituteUserClassId = userClass.Id;
                    studentAttendance.CreatedOn = DateTime.UtcNow;
                    studentAttendance.CreatedBy = $"{user.FirstName} {user.LastName}";
                    studentAttendance.UpdateOn = DateTime.UtcNow;
                    studentAttendance.UpdatedBy = $"{user.FirstName} {user.LastName}";
                    _parentcheckContext.StudentAttendance.Add(studentAttendance);
                    await _parentcheckContext.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }
        
        public async Task<bool> SaveIncidentReportAsync(long instituteUserId, string subject, string message, long userId)
        {
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                IncidentReport incidentReport = new IncidentReport();
                incidentReport.InstituteUserId = instituteUserId;
                incidentReport.InstituteId = user.InstituteId;
                incidentReport.RecordDate = DateTime.Now;
                incidentReport.Subject = subject;
                incidentReport.Message = message;
                incidentReport.ResponsibleUserId = user.Id;
                incidentReport.CreatedOn = DateTime.UtcNow;
                incidentReport.CreatedBy = $"{user.FirstName} {user.LastName}";
                incidentReport.UpdateOn = DateTime.UtcNow;
                incidentReport.UpdatedBy = $"{user.FirstName} {user.LastName}";
                _parentcheckContext.IncidentReport.Add(incidentReport);
                await _parentcheckContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<List<IncidentReportDTO>> GetIncidentReports(long userId)
        {
            List<IncidentReportDTO> incidentReportDTOs = new List<IncidentReportDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var incidentResponsibleUserReports = await (from sa in _parentcheckContext.IncidentReport
                                                            join iu in _parentcheckContext.InstituteUser on sa.ResponsibleUserId equals iu.Id
                                                            where sa.InstituteUserId == user.Id
                                                            select new
                                                            {
                                                                sa.Id,
                                                                sa.InstituteId,
                                                                sa.RecordDate,
                                                                sa.Subject,
                                                                sa.Message,
                                                                sa.InstituteUserId,
                                                                sa.ResponsibleUserId,
                                                                iu.FirstName,
                                                                iu.LastName
                                                            }).ToListAsync();

                foreach (var incidentReport in incidentResponsibleUserReports)
                {
                    incidentReportDTOs.Add(new IncidentReportDTO
                    {
                        Id = incidentReport.Id,
                        InstituteId = incidentReport.InstituteId,
                        InstituteUserId = incidentReport.InstituteUserId,
                        RecordDate = incidentReport.RecordDate,
                        Subject = incidentReport.Subject,
                        Message = incidentReport.Message,
                        UserFirstName = user.FirstName,
                        UserLastName = user.LastName,
                        ResponsibleUserFirstName = incidentReport.FirstName,
                        ResponsibleUserLastName = incidentReport.LastName
                    });
                }

                var incidentInstituteUserReports = await (from sa in _parentcheckContext.IncidentReport
                                                          join iu in _parentcheckContext.InstituteUser on sa.InstituteUserId equals iu.Id
                                                          where sa.ResponsibleUserId == user.Id
                                                          select new
                                                          {
                                                              sa.Id,
                                                              sa.InstituteId,
                                                              sa.RecordDate,
                                                              sa.Subject,
                                                              sa.Message,
                                                              sa.InstituteUserId,
                                                              sa.ResponsibleUserId,
                                                              iu.FirstName,
                                                              iu.LastName
                                                          }).ToListAsync();

                foreach (var incidentReport in incidentInstituteUserReports)
                {
                    incidentReportDTOs.Add(new IncidentReportDTO
                    {
                        Id = incidentReport.Id,
                        InstituteId = incidentReport.InstituteId,
                        InstituteUserId = incidentReport.InstituteUserId,
                        RecordDate = incidentReport.RecordDate,
                        Subject = incidentReport.Subject,
                        Message = incidentReport.Message,
                        ResponsibleUserFirstName = user.FirstName,
                        ResponsibleUserLastName = user.LastName,
                        UserFirstName = incidentReport.FirstName,
                        UserLastName = incidentReport.LastName
                    });
                }
            }


            return incidentReportDTOs.OrderByDescending(i => i.RecordDate).ToList();
        }
    }
}
