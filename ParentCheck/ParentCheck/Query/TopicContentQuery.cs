using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class TopicContentQuery : IRequest<AcademicTopicContentEnvelop>
    {
        public TopicContentQuery(long topicId, long userId)
        {
            this.TopicId = topicId;
            this.UserId = userId;
        }

        public long TopicId { get; set; }
        public long UserId { get; set; }
    }
}
