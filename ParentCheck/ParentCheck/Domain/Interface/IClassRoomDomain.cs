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
    }
}
