using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class AcademicQuery : IRequest<AcademicEnvelop>
    {
        public AcademicQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
