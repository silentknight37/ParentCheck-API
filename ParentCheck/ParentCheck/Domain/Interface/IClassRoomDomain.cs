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
        Task<UserSubmitedAssignmentFileDTO> GetSubmitedAssignmentFileAsync(long userId, long assignmentId);
        Task<bool> UploadAssignmentFileAsync(long assignmentId, string encryptedFileName, string uploadPath, string fileName, long userId);
        Task<long> RemoveAssignmentFileAsync(long submissionId,long id);
        Task<bool> CompleteAssignment(long assignmentId, long userId);
    }
}
