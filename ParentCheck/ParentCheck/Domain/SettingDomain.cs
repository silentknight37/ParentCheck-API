using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public class SettingDomain : ISettingDomain
    {
        private readonly ISettingRepository supportTicketRepository;

        public SettingDomain(ISettingRepository supportTicketRepository)
        {
            this.supportTicketRepository = supportTicketRepository;
        }

        public async Task<List<InstituteUserDTO>> GeInstituteUsers(string searchValue, long userId)
        {
            return await supportTicketRepository.GeInstituteUsers(searchValue, userId);
        }

        public async Task<bool> SaveInstituteUser(
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
            long userId)
        {
            return await supportTicketRepository.SaveInstituteUser(
                id, 
                firstName, 
                lastName, 
                roleId, 
                username, 
                dateOfBirth,
                mobile,
                admission, 
                password,
                parentId,
                parentFirstName,
                parentLastName,
                parentUsername,
                parentDateOfBirth,
                parentMobile,
                parentPassword,
                isActive, 
                userId);
        }

        public async Task<bool> SaveDriviceToken(string deviceToken, long userId)
        {
            return await supportTicketRepository.SaveDriviceToken(deviceToken, userId);
        }
        public async Task<bool> UploadProfileImage(string encryptedFileName, string uploadPath, string fileName, long userId)
        {
            return await supportTicketRepository.UploadProfileImage(encryptedFileName, uploadPath, fileName, userId);
        }

        public async Task<List<PerformanceDTO>> GetPerformance(long userId)
        {
            return await supportTicketRepository.GetPerformance(userId);
        }

        public async Task<List<AcademicDTO>> GetAcademicYear(long userId)
        {
            return await supportTicketRepository.GetAcademicYear(userId);
        }

        public async Task<bool> SaveAcademicYear(long id, int yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveAcademicYear(id, yearAcademic, fromDate, toDate, isActive, userId);
        }

        public async Task<List<AcademicTermDTO>> GetAcademicTerm(long userId)
        {
            return await supportTicketRepository.GetAcademicTerm(userId);
        }

        public async Task<bool> SaveAcademicTerm(long id,string term, long yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveAcademicTerm(id, term, yearAcademic, fromDate, toDate, isActive, userId);
        }

        public async Task<List<AcademicClassDTO>> GetAcademicClass(long userId)
        {
            return await supportTicketRepository.GetAcademicClass(userId);
        }

        public async Task<AcademicClassDTO> GetAcademicClass(long yearAcademic, string academicClass, long userId)
        {
            return await supportTicketRepository.GetAcademicClass(yearAcademic, academicClass, userId);
        }

        public async Task<bool> SaveAcademicClass(long id, string academicClass, long yearAcademic, long responsibleUserId, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveAcademicClass(id, academicClass, yearAcademic, responsibleUserId, isActive, userId);
        }

        public async Task<List<SubjectDTO>> GetSubject(long userId)
        {
            return await supportTicketRepository.GetSubject(userId);
        }

        public async Task<SubjectDTO> GetSubject(string subject,long userId)
        {
            return await supportTicketRepository.GetSubject(subject,userId);
        }

        public async Task<bool> SaveSubject(long id, string subject, string descriptionText, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveSubject(id, subject, descriptionText, isActive, userId);
        }

        public async Task<List<StudentEnrollDTO>> GetStudentEnroll(long classId, long academicYear, long userId)
        {
            return await supportTicketRepository.GetStudentEnroll(classId, academicYear, userId);
        }

        public async Task<StudentEnrollDTO> GetStudentEnroll(long classId, long studentId, long academicYear, long userId)
        {
            return await supportTicketRepository.GetStudentEnroll(classId, studentId,academicYear, userId);
        }
        public async Task<bool> SaveStudentEnroll(long id, long yearAcademic, long classId, long studentId, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveStudentEnroll(id, yearAcademic, classId, studentId, isActive, userId);
        }

        public async Task<List<AcademicClassSubjectDTO>> GetClassSubject(long classId, long userId)
        {
            return await supportTicketRepository.GetClassSubject(classId,userId);
        }
        public async Task<bool> SaveClassSubject(long id, long classId, long subjectId, long responsibleUserId, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveClassSubject(id, classId, subjectId, responsibleUserId, isActive, userId);
        }

        public async Task<List<SubjectChapterDTO>> GetSubjectChapter(long subjectId, long userId)
        {
            return await supportTicketRepository.GetSubjectChapter(subjectId,userId);
        }

        public async Task<bool> SaveSubjectChapter(long id, long subjectId, string chapter, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveSubjectChapter(id, subjectId, chapter, isActive, userId);
        }

        public async Task<List<ChapterTopicsDTO>> GetChapterTopic(long chapterId, long userId)
        {
            return await supportTicketRepository.GetChapterTopic(chapterId, userId);
        }

        public async Task<bool> SaveChapterTopic(long id, long chapterId, string topic, string description, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveChapterTopic(id, chapterId, topic, description, isActive, userId);
        }

        public async Task<List<TopicContentDTO>> GetTopicContent(long topicId, long userId)
        {
            return await supportTicketRepository.GetTopicContent(topicId, userId);
        }

        public async Task<bool> SaveTopicContent(long id, long topicId, string contentText, int contentTypeId, int orderId, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveTopicContent(id, topicId, contentText, contentTypeId, orderId,isActive, userId);
        }

        public async Task<bool> UploadTopicContent(long topicContentId, string encryptedFileName, string uploadPath, string fileName, long userId)
        {
            return await supportTicketRepository.UploadTopicContent(topicContentId,encryptedFileName, uploadPath, fileName, userId);
        }
        public async Task<bool> RemoveTopicContent(long topicContentId, long id)
        {
            return await supportTicketRepository.RemoveTopicContent(topicContentId, id);
        }

        public async Task<List<AssociateClassDTO>> GetAssociateClass(long userId)
        {
            return await supportTicketRepository.GetAssociateClass(userId);
        }

        public async Task<List<WeekDayDTO>> GetTimeTable(bool isOnlyToday, long userId)
        {
            return await supportTicketRepository.GetTimeTable(isOnlyToday,userId);
        }

        public async Task<List<WeekDayDTO>> GetAllTimeTable(long classId, long userId)
        {
            return await supportTicketRepository.GetAllTimeTable(classId, userId);
        }

        public async Task<bool> SaveTimeTable(long id, long classId, long subjectId, string fromTime, string toTime, int weekDayId, long userId)
        {
            return await supportTicketRepository.SaveTimeTable(id, classId, subjectId, fromTime, toTime, weekDayId, userId);
        }
    }
}
