using MediatR;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Factory;
using ParentCheck.Factory.Intreface;
using ParentCheck.Query;
using System.Threading;
using System.Threading.Tasks;

namespace ParentCheck.Handler
{
    public class ReferenceQueryHandler : IRequestHandler<ReferenceQuery, ReferenceEnvelop>
    {
        private readonly IReferenceFactory referenceFactory;

        public ReferenceQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.referenceFactory = new ReferenceFactory(parentcheckContext);
        }

        public async Task<ReferenceEnvelop> Handle(ReferenceQuery referenceQuery,CancellationToken cancellationToken)
        {
            var referenceDomain = this.referenceFactory.Create();
            var references = await referenceDomain.GetReferenceByTypeAsync(referenceQuery.ContextId,referenceQuery.ReferenceTypeId, referenceQuery.UserId);

            return new ReferenceEnvelop(references);
        }
    }
}
