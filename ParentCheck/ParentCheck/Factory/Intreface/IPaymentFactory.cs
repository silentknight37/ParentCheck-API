using ParentCheck.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParentCheck.Factory.Intreface
{
    public interface IPaymentFactory
    {
        IPaymentDomain Create();
    }
}
