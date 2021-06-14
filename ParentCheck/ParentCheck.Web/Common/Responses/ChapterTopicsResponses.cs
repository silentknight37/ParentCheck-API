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
        public List<ChapterTopic> chapterTopics { get; set; }

        public static ChapterTopicsResponses PopulateChapterTopicsResponses(UserChapterTopicsDTO userChapterTopics)
        {
            var chapterTopicsResponses = new ChapterTopicsResponses();
            chapterTopicsResponses.chapterTopics = new List<ChapterTopic>();
            chapterTopicsResponses.chapter = userChapterTopics.Chapter;

            foreach (var ChapterTopic in userChapterTopics.ChapterTopics)
            {
                var topic = new ChapterTopic
                {
                    id= ChapterTopic.InstituteChapterTopicId,
                    topic = ChapterTopic.Topic,
                    description= ChapterTopic.Description
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
    }
}
