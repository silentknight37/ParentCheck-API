using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public interface IClassRoomDomain
    {
        Task<UserClassDTO> GetUserSubjectAsync(long userId);
        Task<UserSubjectChapterDTO> GetUserSubjectChaptersAsync(long classSubjectId, long userId);
        Task<UserChapterTopicsDTO> GetUserChaptersTopicsAsync(long subjectChapterId, long userId);
        Task<UserTopicContentsDTO> GetUserTopicContentAsync(long chapterTopicId, long userId);
        Task<UserSubmitedAssignmentFileDTO> GetSubmitedAssignmentFileAsync(long assignmentId, long userId);
        Task<bool> UploadAssignmentFileAsync(long assignmentId, string encryptedFileName, string uploadPath, string fileName, long userId);
        Task<long> RemoveAssignmentFileAsync(long submissionId,long id);
        Task<bool> CompleteAssignment(long assignmentId, long userId);
        Task<List<ClassRoomOverviewDTO>> GetClassRoomOverviewAsync(bool isToday, bool isThisWeek, bool isNextWeek, bool isCustom, DateTime? fromDate, DateTime? toDate, long? subjectId, long? instituteTermsId, long userId);
        Task<List<LibraryDTO>> GetLibraryAsync(long userId);
        Task<bool> UploadLibrayFileAsync(long instituteId, string libraryDescription, string encryptedFileName, string uploadPath, string fileName, bool isInstituteLevel, int contentType, long userId);
    }
}
