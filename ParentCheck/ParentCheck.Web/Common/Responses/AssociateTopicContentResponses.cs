using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class AssociateTopicContentResponses
    {
        public List<AssociateChapterTopic> topicContents { get; set; }

        public static AssociateTopicContentResponses PopulateAssociateTopicContentResponses(List<TopicContentDTO> topicContentDTOs)
        {
            var associateTopicContentResponses = new AssociateTopicContentResponses();

            associateTopicContentResponses.topicContents = new List<AssociateChapterTopic>();

            foreach (var topicContentDTO in topicContentDTOs)
            {

                var topicContent = new AssociateChapterTopic
                {
                    id = topicContentDTO.InstituteTopicContentId,
                    contentTypeId = topicContentDTO.ContentTypeId,
                    contentText = topicContentDTO.ContentText,
                    contentType = topicContentDTO.ContentType,
                    orderId = topicContentDTO.ContentOrder,
                    isActive = topicContentDTO.IsActive
                };

                foreach (var contentDocument in topicContentDTO.ContentDocuments)
                {
                    topicContent.contentDocuments.Add(new AssociateContentDocument
                    {
                        id = contentDocument.Id,
                        fileName = contentDocument.FileName,
                        enFileName = contentDocument.EncryptedFileName,
                        topicContentId = contentDocument.InstituteTopicContentId,
                        url = contentDocument.Url
                    });
                }

                associateTopicContentResponses.topicContents.Add(topicContent);
            }

            return associateTopicContentResponses;
        }
    }

    public class AssociateChapterTopic
    {
        public AssociateChapterTopic()
        {
            contentDocuments = new List<AssociateContentDocument>();
        }

        public long id { get; set; }
        public int contentTypeId { get; set; }
        public string contentType { get; set; }
        public string contentText { get; set; }
        public int orderId { get; set; }
        public bool isActive { get; set; }
        public List<AssociateContentDocument> contentDocuments { get; set; }
    }

    public class AssociateContentDocument
    {
        public long id { get; set; }
        public long topicContentId { get; set; }
        public string fileName { get; set; }
        public string enFileName { get; set; }
        public string url { get; set; }
    }
}
