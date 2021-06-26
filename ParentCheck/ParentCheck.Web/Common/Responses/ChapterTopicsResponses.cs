using ParentCheck.BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParentCheck.Web.Common.Responses
{
    public class ChapterTopicsResponses
    {
        public string chapter { get; set; }
        public string bgColor { get; set; }
        public string fontColor { get; set; }
        public bool isAssignmentAssign { get; set; }
        public Assignment assignment { get; set; }
        public List<ChapterTopic> chapterTopics { get; set; }

        public static ChapterTopicsResponses PopulateChapterTopicsResponses(UserChapterTopicsDTO userChapterTopics)
        {
            var chapterTopicsResponses = new ChapterTopicsResponses();
            chapterTopicsResponses.chapterTopics = new List<ChapterTopic>();
            chapterTopicsResponses.chapter = userChapterTopics.Chapter;
            chapterTopicsResponses.bgColor = userChapterTopics.ColorBg;
            chapterTopicsResponses.fontColor = userChapterTopics.ColorFont;
            chapterTopicsResponses.isAssignmentAssign = userChapterTopics.IsAssignmentAssign;
            if (userChapterTopics.IsAssignmentAssign)
            {
                var assignment = new Assignment
                {
                    id = userChapterTopics.Assignment.Id,
                    title = userChapterTopics.Assignment.AssignmentTitle,
                    openDate = userChapterTopics.Assignment.OpenDate,
                    closeDate = userChapterTopics.Assignment.CloseDate,
                    description = userChapterTopics.Assignment.AssignmentDescription,
                };
                userChapterTopics.Assignment.AssignmentDocuments.ForEach(i =>
                assignment.documents.Add(
                new AssignmentDocument
                {
                    id = i.Id,
                    assignmentId = i.InstituteAssignmentId,
                    fileName = i.FileName,
                    url = i.Url,
                    typeId=i.AssignmentTypeId
                }));

                chapterTopicsResponses.assignment = assignment;
            }

            foreach (var ChapterTopic in userChapterTopics.ChapterTopics)
            {
                var topic = new ChapterTopic
                {
                    id= ChapterTopic.InstituteChapterTopicId,
                    topic = ChapterTopic.Topic,
                    description= ChapterTopic.Description,
                    submitDate= ChapterTopic.SubmitDate
                };

                chapterTopicsResponses.chapterTopics.Add(topic);
            }

            return chapterTopicsResponses;
        }
    }

    public class ChapterTopic
    {
        public long id { get; set; }
        public string topic { get; set; }
        public string description { get; set; }
        public string submitDate { get; set; }
    }
}
