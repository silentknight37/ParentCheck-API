using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class UserChapterTopicsEnvelop
    {
        public UserChapterTopicsEnvelop(UserChapterTopicsDTO userChapterTopics)
        {
            this.UserChapterTopics = userChapterTopics;
        }

        public UserChapterTopicsDTO UserChapterTopics { get; set; }
    }
}
