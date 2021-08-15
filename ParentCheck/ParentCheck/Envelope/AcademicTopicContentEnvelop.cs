using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class AcademicTopicContentEnvelop
    {
        public AcademicTopicContentEnvelop(List<TopicContentDTO> topicContentDTOs)
        {
            this.topicContents = topicContentDTOs;
        }

        public List<TopicContentDTO> topicContents { get; set; }
    }
}
