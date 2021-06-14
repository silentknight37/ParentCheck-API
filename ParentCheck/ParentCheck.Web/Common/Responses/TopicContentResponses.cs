using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class TopicContentResponses
    {
        public string topic { get; set; }
        public List<TopicContent> topicContents { get; set; }

        public static TopicContentResponses PopulateChapterTopicsResponses(UserTopicContentsDTO userTopicContents)
        {
            var topicContentResponses = new TopicContentResponses();
            topicContentResponses.topicContents = new List<TopicContent>();
            topicContentResponses.topic = userTopicContents.Topic;

            foreach (var topicContent in userTopicContents.TopicContents)
            {
                var content = new TopicContent
                {
                    id= topicContent.InstituteTopicContentId,
                    typeId= topicContent.ContentTypeId,
                    type= topicContent.ContentType,
                    description= topicContent.ContentText,
                    url= topicContent.ContentURL,
                    order = topicContent.ContentOrder
                };

                topicContentResponses.topicContents.Add(content);
            }

            return topicContentResponses;
        }
    }

    public class TopicContent
    {
        public long id { get; set; }
        public int typeId { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public int order { get; set; }
    }
}
