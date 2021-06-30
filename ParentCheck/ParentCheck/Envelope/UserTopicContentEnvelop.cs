using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Envelope
{
    public class UserTopicContentEnvelop
    {
        public UserTopicContentEnvelop(UserTopicContentsDTO userTopicContents)
        {
            this.UserTopicContents = userTopicContents;
        }

        public UserTopicContentsDTO UserTopicContents { get; set; }
    }
}
