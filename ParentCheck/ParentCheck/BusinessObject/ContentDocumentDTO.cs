using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class ContentDocumentDTO
    {
        public long Id { get; set; }
        public long InstituteTopicContentId { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
    }
}
