using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class ChapterTopicSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public ChapterTopicSaveCommand(long id, long chapterId, string topic, string description, bool isActive, long userId)
        {
            this.Id = id;
            this.ChapterId = chapterId;
            this.Topic = topic;
            this.Description = description;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public long ChapterId { get; set; }
        public string Topic { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
