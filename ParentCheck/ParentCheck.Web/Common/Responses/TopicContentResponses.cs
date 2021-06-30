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
        public string bgColor { get; set; }
        public string fontColor { get; set; }
        public long subjectId { get; set; }
        public bool isAssignmentAssign { get; set; }
        public Assignment assignment { get; set; }
        public List<TopicContent> topicContents { get; set; }

        public static TopicContentResponses PopulateChapterTopicsResponses(UserTopicContentsDTO userTopicContents)
        {
            var topicContentResponses = new TopicContentResponses();
            topicContentResponses.topicContents = new List<TopicContent>();
            topicContentResponses.topic = userTopicContents.Topic;
            topicContentResponses.bgColor = userTopicContents.ColorBg;
            topicContentResponses.fontColor = userTopicContents.ColorFont;
            topicContentResponses.subjectId = userTopicContents.SubjectId;
            topicContentResponses.isAssignmentAssign = userTopicContents.IsAssignmentAssign;

            if (userTopicContents.IsAssignmentAssign)
            {
                var assignment= new Assignment
                {
                    id = userTopicContents.Assignment.Id,
                    title = userTopicContents.Assignment.AssignmentTitle,
                    openDate = userTopicContents.Assignment.OpenDate,
                    closeDate = userTopicContents.Assignment.CloseDate,
                    description = userTopicContents.Assignment.AssignmentDescription,
                };
                userTopicContents.Assignment.AssignmentDocuments.ForEach(i =>
                assignment.documents.Add(
                new AssignmentDocument
                {
                    id = i.Id,
                    assignmentId = i.InstituteAssignmentId,
                    fileName = i.FileName,
                    url = i.Url,
                    typeId=i.AssignmentTypeId
                }));

                topicContentResponses.assignment = assignment;
            }

            foreach (var topicContent in userTopicContents.TopicContents)
            {
                var content = new TopicContent
                {
                    id = topicContent.InstituteTopicContentId,
                    typeId = topicContent.ContentTypeId,
                    type = topicContent.ContentType,
                    description = topicContent.ContentText,
                    order = topicContent.ContentOrder
                };

                topicContent.ContentDocuments.ForEach(i => content.documents.Add(
                    new ContentDocument
                    {
                        id = i.Id,
                        topicContentId = i.InstituteTopicContentId,
                        fileName = i.FileName,
                        url = i.Url
                    }));


                topicContentResponses.topicContents.Add(content);
            }

            return topicContentResponses;
        }
    }

    public class TopicContent
    {
        public TopicContent()
        {
            documents = new List<ContentDocument>();
        }
        public long id { get; set; }
        public int typeId { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public int order { get; set; }
        public List<ContentDocument> documents { get; set; }
    }

    public class ContentDocument
    {
        public long id { get; set; }
        public long topicContentId { get; set; }
        public string fileName { get; set; }
        public string url { get; set; }
    }

    public class Assignment
    {
        public Assignment()
        {
            documents = new List<AssignmentDocument>();
        }
        public long id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime openDate { get; set; }
        public DateTime closeDate { get; set; }
        public List<AssignmentDocument> documents { get; set; }
    }

    public class AssignmentDocument
    {
        public long id { get; set; }
        public long assignmentId { get; set; }
        public string fileName { get; set; }
        public int typeId { get; set; }
        public string type { get; set; }
        public string url { get; set; }
    }
}
