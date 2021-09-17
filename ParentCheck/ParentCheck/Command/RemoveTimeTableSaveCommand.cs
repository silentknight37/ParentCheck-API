using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class RemoveTimeTableSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public RemoveTimeTableSaveCommand(long id, long userId)
        {
            this.Id = id;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public long UserId { get; set; }
    }
}
