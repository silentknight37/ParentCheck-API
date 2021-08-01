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

        public async Task<long> RemoveAssignmentFileAsync(long submissionId,long id)
        {
            return await _classRoomRepository.RemoveAssignmentFileAsync(submissionId,id);
        }

        public async Task<bool> CompleteAssignment(long assignmentId, long userId)
        {
            return await _classRoomRepository.CompleteAssignment(assignmentId, userId);
        }
    }
}
