using MediatR;
using ParentCheck.Common;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class CommunicationTemplateQuery : IRequest<CommunicationTemplateEnvelop>
    {
        public CommunicationTemplateQuery(bool isActiveOnly, long userId)
        {
            this.IsActiveOnly = isActiveOnly;
            this.UserId = userId;
        }

        public bool IsActiveOnly { get; set; }
        public long UserId { get; set; }
    }
}
