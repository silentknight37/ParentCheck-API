using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class LibraryQuery : IRequest<LibraryEnvelop>
    {
        public LibraryQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
