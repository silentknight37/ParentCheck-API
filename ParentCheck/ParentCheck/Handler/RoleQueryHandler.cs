using MediatR;
using ParentCheck.Data;
using ParentCheck.Envelope;
using ParentCheck.Query;
using ParentCheck.Factory;
using ParentCheck.Factory.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ParentCheck.Repository.Intreface;

namespace ParentCheck.Handler
{
    public class RoleQueryHandler:IRequestHandler<RoleQuery, RoleEnvelop>
    {
        private readonly IRoleFactory roleFactory;

        public RoleQueryHandler(ParentCheckContext parentcheckContext)
        {
            this.roleFactory = new RoleFactory(parentcheckContext);
        }

        public async Task<RoleEnvelop> Handle(RoleQuery roleQuery,CancellationToken cancellationToken)
        {
            var roleFactory = this.roleFactory.Create();
            return new RoleEnvelop();
        }
    }
}
