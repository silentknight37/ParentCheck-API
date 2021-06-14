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
    }
}
