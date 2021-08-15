using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Models
{
    public class ChapterTopicRequest
    {
        public long id { get; set; }
        public long chapterId { get; set; }
        public string topic { get; set; }
        public string description { get; set; }
        public bool isActive { get; set; }
    }
}
