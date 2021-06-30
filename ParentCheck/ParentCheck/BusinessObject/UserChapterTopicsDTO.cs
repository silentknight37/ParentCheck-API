using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class UserChapterTopicsDTO
    {
        public UserChapterTopicsDTO()
        {
            ChapterTopics = new List<ChapterTopicsDTO>();
        }

        public string Chapter { get; set; }
        public AssignmentDTO Assignment { get; set; }
        public bool IsAssignmentAssign { get; set; }
        public string ColorBg { get; set; }
        public string ColorFont { get; set; }
        public List<ChapterTopicsDTO> ChapterTopics { get; set; }
    }
}
