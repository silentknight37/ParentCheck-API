using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class SubjectChapterResponses
    {
        public string subject { get; set; }
        public string bgColor { get; set; }
        public string fontColor { get; set; }
        public List<SubjectChapter> subjectChapters { get; set; }

        public static SubjectChapterResponses PopulateSubjectChapterResponses(UserSubjectChapterDTO subjectChapters)
        {
            var subjectResponses = new SubjectChapterResponses();
            subjectResponses.subjectChapters = new List<SubjectChapter>();
            subjectResponses.subject = subjectChapters.Subject;
            subjectResponses.bgColor = subjectChapters.ColorBg;
            subjectResponses.fontColor = subjectChapters.ColorFont;

            foreach (var chapter in subjectChapters.Chapters)
            {
                var subjectChapter = new SubjectChapter
                {
                    id= chapter.InstituteSubjectChapterId,
                    chapterText = chapter.Chapter,
                    topicCount=chapter.TopicCount
                };

                subjectResponses.subjectChapters.Add(subjectChapter);
            }

            return subjectResponses;
        }
    }

    public class SubjectChapter
    {
        public long id { get; set; }
        public string chapterText { get; set; }
        public int topicCount { get; set; }
    }
}
