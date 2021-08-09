using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class AcademicClassQuery : IRequest<AcademicClassEnvelop>
    {
        public AcademicClassQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
