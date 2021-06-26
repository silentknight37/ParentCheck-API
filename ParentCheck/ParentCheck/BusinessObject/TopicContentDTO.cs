using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class TopicContentDTO
    {
        public TopicContentDTO()
        {
            ContentDocuments = new List<ContentDocumentDTO>();
        }

        public long InstituteTopicContentId { get; set; }
        public int ContentTypeId { get; set; }
        public string ContentType { get; set; }
        public string ContentText { get; set; }
        public int ContentOrder { get; set; }
        public List<ContentDocumentDTO> ContentDocuments { get; set; }
    }
}
