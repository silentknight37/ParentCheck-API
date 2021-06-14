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
        public List<ChapterTopicsDTO> ChapterTopics { get; set; }
    }
}
