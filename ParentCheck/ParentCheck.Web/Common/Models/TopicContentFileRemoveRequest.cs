using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class TopicContentFileRemoveRequest
    {
        public long id { get; set; }
        public long topicContentId { get; set; }
        public long subjectChapterId { get; set; }
        public long chapterTopicId { get; set; }
        public string fileName { get; set; }
        public string enFileName { get; set; }
        public string url { get; set; }
    }
}
