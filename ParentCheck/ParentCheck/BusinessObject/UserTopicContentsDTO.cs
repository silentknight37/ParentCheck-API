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
        public AssignmentDTO Assignment { get; set; }
        public bool IsAssignmentAssign { get; set; }
        public long SubjectId { get; set; }
        public string ColorBg { get; set; }
        public string ColorFont { get; set; }
        public List<TopicContentDTO> TopicContents { get; set; }
    }
}
