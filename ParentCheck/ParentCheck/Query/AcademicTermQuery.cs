using MediatR;
using ParentCheck.Envelope;

namespace ParentCheck.Query
{
    public class AcademicTermQuery : IRequest<AcademicTermEnvelop>
    {
        public AcademicTermQuery(long userId)
        {
            this.UserId = userId;
        }

        public long UserId { get; set; }
    }
}
