using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class AssociateChapterTopicResponses
    {
        public List<ChapterTopics> chapterTopics { get; set; }

        public static AssociateChapterTopicResponses PopulateAssociateChapterTopicResponses(List<ChapterTopicsDTO> chapterTopicsDTOs)
        {
            var associateChapterTopicResponses = new AssociateChapterTopicResponses();

            associateChapterTopicResponses.chapterTopics = new List<ChapterTopics>();

            foreach (var chapterTopicsDTO in chapterTopicsDTOs)
            {

                var chapterTopic = new ChapterTopics
                {
                    id = chapterTopicsDTO.InstituteChapterTopicId,
                    assignmentId= chapterTopicsDTO.InstituteAssignmentId,
                    topic= chapterTopicsDTO.Topic,
                    description= chapterTopicsDTO.Description,
                    createDate = chapterTopicsDTO.SubmitDate,
                    isAssignmentAssigned = chapterTopicsDTO.IsAssignmentAssigned,
                    isActive = chapterTopicsDTO.IsActive
                };

                associateChapterTopicResponses.chapterTopics.Add(chapterTopic);
            }

            return associateChapterTopicResponses;
        }
    }

    public class ChapterTopics
    {
        public long id { get; set; }
        public string topic { get; set; }
        public string description { get; set; }
        public string createDate { get; set; }
        public bool isActive { get; set; }
        public bool isAssignmentAssigned { get; set; }
        public long? assignmentId { get; set; }
    }
}
