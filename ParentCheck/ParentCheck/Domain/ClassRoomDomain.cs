using ParentCheck.BusinessObject;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public class ClassRoomDomain : IClassRoomDomain
    {
        private readonly IClassRoomRepository _classRoomRepository;

        public ClassRoomDomain(IClassRoomRepository classRoomRepository)
        {
            _classRoomRepository = classRoomRepository;
        }

        public async Task<UserClassDTO> GetUserSubjectAsync(long userId)
        {
            return await _classRoomRepository.GetUserSubjectAsync(userId);
        }

        public async Task<UserSubjectChapterDTO> GetUserSubjectChaptersAsync(long classSubjectId,long userId)
        {
            return await _classRoomRepository.GetUserSubjectChaptersAsync(classSubjectId, userId);
        }

        public async Task<UserChapterTopicsDTO> GetUserChaptersTopicsAsync(long subjectChapterId, long userId)
        {
            return await _classRoomRepository.GetUserChaptersTopicsAsync(subjectChapterId, userId);
        }

        public async Task<UserTopicContentsDTO> GetUserTopicContentAsync(long chapterTopicId, long userId)
        {
            return await _classRoomRepository.GetUserTopicContentAsync(chapterTopicId, userId);
        }

        public async Task<UserSubmitedAssignmentFileDTO> GetSubmitedAssignmentFileAsync(long assignmentId, long userId)
        {
            return await _classRoomRepository.GetSubmitedAssignmentFileAsync(assignmentId, userId);
        }

        public async Task<bool> UploadAssignmentFileAsync(long assignmentId, string encryptedFileName, string uploadPath, string fileName, long userId)
        {
            return await _classRoomRepository.UploadAssignmentFileAsync(assignmentId, encryptedFileName, uploadPath, fileName, userId);
        }

        public async Task<bool> UploadLibrayFileAsync(long instituteId, string libraryDescription, string encryptedFileName, string uploadPath, string fileName, bool isInstituteLevel,int contentType, long userId)
        {
            return await _classRoomRepository.UploadLibrayFileAsync(instituteId, libraryDescription, encryptedFileName, uploadPath, fileName, isInstituteLevel,contentType, userId);
        }

        public async Task<long> RemoveAssignmentFileAsync(long submissionId,long id)
        {
            return await _classRoomRepository.RemoveAssignmentFileAsync(submissionId,id);
        }

        public async Task<bool> CompleteAssignment(long assignmentId, long userId)
        {
            return await _classRoomRepository.CompleteAssignment(assignmentId, userId);
        }

        public async Task<List<ClassRoomOverviewDTO>> GetClassRoomOverviewAsync(bool isToday, bool isThisWeek, bool isNextWeek, bool isCustom, DateTime? fromDate, DateTime? toDate, long? subjectId, long? instituteTermsId, long userId)
        {
            return await _classRoomRepository.GetClassRoomOverviewAsync(isToday,isThisWeek,isNextWeek,isCustom, fromDate, toDate, subjectId, instituteTermsId,userId);
        }
        public async Task<List<LibraryDTO>> GetLibraryAsync(long userId)
        {
            return await _classRoomRepository.GetLibraryAsync(userId);
        }
        public async Task<List<ClassStudentAttendancesDTO>> GetClassStudentAttendancesAsync(long classId, DateTime recordDate,long userId)
        {
            return await _classRoomRepository.GetClassStudentAttendancesAsync(classId,recordDate, userId);
        }

        public async Task<List<ClassStudentAttendancesDTO>> GetStudentAttendancesAsync(long userId)
        {
            return await _classRoomRepository.GetStudentAttendancesAsync(userId);
        }
        public async Task<List<ClassStudentDTO>> GetClassStudentAsync(long classId, long userId)
        {
            return await _classRoomRepository.GetClassStudentAsync(classId, userId);
        }
        public async Task<bool> SaveClassStudentAttendanceAsync(long instituteUserId, long instituteClassId, DateTime recordDate, bool isAttendance, bool isReset, long userId)
        {
            return await _classRoomRepository.SaveClassStudentAttendanceAsync(instituteUserId, instituteClassId, recordDate, isAttendance,isReset, userId);
        }
        public async Task<bool> SaveIncidentReportAsync(long instituteUserId, string subject, string message, long userId)
        {
            return await _classRoomRepository.SaveIncidentReportAsync(instituteUserId,subject,message, userId);
        }
        public async Task<List<IncidentReportDTO>> GetIncidentReports(long userId)
        {
            return await _classRoomRepository.GetIncidentReports(userId);
        }
    }
}
