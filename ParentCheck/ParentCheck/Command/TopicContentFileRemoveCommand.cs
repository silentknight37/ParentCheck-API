using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class TopicContentFileRemoveCommand : IRequest<RequestSaveEnvelop>
    {
        public TopicContentFileRemoveCommand(long topicContentId, long assignmentFileId, long userId)
        {
            this.TopicContentId = topicContentId;
            this.Id = assignmentFileId;
            this.UserId = userId;
        }

        public long TopicContentId { get; set; }
        public long Id { get; set; }
        public long UserId { get; set; }
    }
}
