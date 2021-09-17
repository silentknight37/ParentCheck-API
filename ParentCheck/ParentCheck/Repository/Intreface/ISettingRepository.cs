using ParentCheck.BusinessObject;
using ParentCheck.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository.Intreface
{
    public interface ISettingRepository
    {
        Task<List<InstituteUserDTO>> GeInstituteUsers(string searchValue, int? roleId, long userId);
        Task<bool> SaveInstituteUser(
            long id,
            string firstName,
            string lastName,
            int roleId,
            string username,
            DateTime dateOfBirth,
            string mobile,
            string admission,
            string password,
            long parentId,
            string parentFirstName,
            string parentLastName,
            string parentUsername,
            DateTime? parentDateOfBirth,
            string parentMobile,
            string parentPassword,
            bool isActive,
            long userId);
        Task<bool> ResetPassword(long id, string password, long userId);
        Task<bool> SaveDriviceToken(string deviceToken, long userId);
        Task<bool> UploadProfileImage(string encryptedFileName, string uploadPath, string fileName, long userId);

        Task<List<PerformanceDTO>> GetPerformance(long userId);

        Task<List<AcademicDTO>> GetAcademicYear(long userId);
        Task<bool> SaveAcademicYear(long id, int yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId);

        Task<List<AcademicTermDTO>> GetAcademicTerm(long userId);
        Task<bool> SaveAcademicTerm(long id, string term, long yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId);

        Task<List<AcademicClassDTO>> GetAcademicClass(long userId);
        Task<AcademicClassDTO> GetAcademicClass(long yearAcademic, string academicClass, long userId);
        Task<bool> SaveAcademicClass(long id, string academicClass, long yearAcademic, long responsibleUserId, bool isActive, long userId);

        Task<List<SubjectDTO>> GetSubject(long userId);
        Task<SubjectDTO> GetSubject(string subject, long userId);
        Task<bool> SaveSubject(long id, string subject, string descriptionText, bool isActive, long userId);

        Task<List<StudentEnrollDTO>> GetStudentEnroll(long classId, long academicYear, long userId);
        Task<StudentEnrollDTO> VerifyStudentEnroll(long studentId, long academicYear, long userId);
        Task<bool> SaveStudentEnroll(long id, long yearAcademic, long classId, long studentId, bool isActive, long userId);

        Task<List<AcademicClassSubjectDTO>> GetClassSubject(long classId, long userId);
        Task<bool> SaveClassSubject(long id, long classId, long subjectId, long responsibleUserId, bool isActive, long userId);

        Task<List<SubjectChapterDTO>> GetSubjectChapter(long subjectId, long userId);
        Task<bool> SaveSubjectChapter(long id, long subjectId, string chapter, bool isActive, long userId);

        Task<List<ChapterTopicsDTO>> GetChapterTopic(long chapterId, long userId);
        Task<bool> SaveChapterTopic(long id, long chapterId, string topic, string description, bool isActive, long userId);

        Task<List<TopicContentDTO>> GetTopicContent(long topicId, long userId);
        Task<bool> SaveTopicContent(long id, long topicId, string contentText, int contentTypeId, int orderId, bool isActive, long userId);
        Task<bool> UploadTopicContent(long topicContentId, string encryptedFileName, string uploadPath, string fileName, long userId);
        Task<bool> RemoveTopicContent(long topicContentId, long id);


        Task<List<AssociateClassDTO>> GetAssociateClass(long userId);
        Task<List<WeekDayDTO>> GetTimeTable(bool isOnlyToday, long userId);
        Task<List<WeekDayDTO>> GetAllTimeTable(long classId, long userId); Task<bool> SaveTimeTable(long id, long classId, long subjectId, string fromTime, string toTime, int weekDayId, long userId);
        Task<bool> RemoveTimeTable(long id, long userId);
    }
}
