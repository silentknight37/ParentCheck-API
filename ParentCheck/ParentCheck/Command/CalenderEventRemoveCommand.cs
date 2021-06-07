using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class CalenderEventRemoveCommand : IRequest<RequestSaveEnvelop>
    {
        public CalenderEventRemoveCommand(long id, long userId)
        {
            this.Id = id;
            this.UserId = userId;
        }

        public long Id { get; set; }
        public long UserId { get; set; }
    }
}
