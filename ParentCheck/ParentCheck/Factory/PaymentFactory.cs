using ParentCheck.Data;
using ParentCheck.Domain;
using ParentCheck.Factory.Intreface;
using ParentCheck.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Factory
{
    public class PaymentFactory : IPaymentFactory
    {
        private ParentCheckContext _parentCheckContext;

        public PaymentFactory(ParentCheckContext parentCheckContext)
        {
            _parentCheckContext = parentCheckContext;
        }

        IPaymentDomain IPaymentFactory.Create()
        {
            return new PaymentDomain(new PaymentRepository(_parentCheckContext));
        }
    }
}
