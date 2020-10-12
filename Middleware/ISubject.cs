
namespace WeirdUnitBE.Middleware
{
    public interface ISubject
    {
        void Attach(IObserver observer);
        void Notify();
    }
}