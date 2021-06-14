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
        public List<SubjectChapter> subjectChapters { get; set; }

        public static SubjectChapterResponses PopulateSubjectChapterResponses(UserSubjectChapterDTO subjectChapters)
        {
            var subjectResponses = new SubjectChapterResponses();
            subjectResponses.subjectChapters = new List<SubjectChapter>();
            subjectResponses.subject = subjectChapters.Subject;

            foreach (var chapter in subjectChapters.Chapters)
            {
                var subjectChapter = new SubjectChapter
                {
                    id= chapter.InstituteSubjectChapterId,
                    chapterText = chapter.Chapter
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
    }
}
