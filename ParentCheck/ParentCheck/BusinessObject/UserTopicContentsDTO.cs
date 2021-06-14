using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.BusinessObject
{
    public class UserTopicContentsDTO
    {
        public UserTopicContentsDTO()
        {
            TopicContents = new List<TopicContentDTO>();
        }

        public string Topic { get; set; }
        public List<TopicContentDTO> TopicContents { get; set; }
    }
}
