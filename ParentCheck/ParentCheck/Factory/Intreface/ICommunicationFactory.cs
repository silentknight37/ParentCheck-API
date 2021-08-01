using ParentCheck.Domain;

namespace ParentCheck.Factory.Intreface
{
    public interface ICommunicationFactory
    {
        ICommunicationDomain Create();
    }
}
