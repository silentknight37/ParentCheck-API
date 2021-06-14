using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class UserSubjectChapterEnvelop
    {
        public UserSubjectChapterEnvelop(UserSubjectChapterDTO userSubjectChapter)
        {
            this.UserSubjectChapter = userSubjectChapter;
        }

        public UserSubjectChapterDTO UserSubjectChapter { get; set; }
    }
}
