using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class UserSubjectChapterDTO
    {
        public UserSubjectChapterDTO()
        {
            Chapters = new List<SubjectChapterDTO>();
        }

        public string Subject { get; set; }
        public string ColorBg { get; set; }
        public string ColorFont { get; set; }
        public List<SubjectChapterDTO> Chapters { get; set; }
    }
}
