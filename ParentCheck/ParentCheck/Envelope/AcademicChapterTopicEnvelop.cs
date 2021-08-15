using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class AcademicChapterTopicEnvelop
    {
        public AcademicChapterTopicEnvelop(List<ChapterTopicsDTO> chapterTopicsDTOs)
        {
            this.chapterTopics = chapterTopicsDTOs;
        }

        public List<ChapterTopicsDTO> chapterTopics { get; set; }
    }
}
