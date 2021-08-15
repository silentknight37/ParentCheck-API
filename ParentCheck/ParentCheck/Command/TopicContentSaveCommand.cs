using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class TopicContentSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public TopicContentSaveCommand(long id, long topicId, string contentText, int contentTypeId,int orderId, bool isActive, long userId)
        {
            this.Id = id;
            this.TopicId = topicId;
            this.ContentText = contentText;
            this.ContentTypeId = contentTypeId;
            this.OrderId = orderId;
            this.IsActive = isActive;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public long TopicId { get; set; }
        public string ContentText { get; set; }
        public int ContentTypeId { get; set; }
        public int OrderId { get; set; }
        public bool IsActive { get; set; }
        public long UserId { get; set; }
    }
}
