using System.Threading.Tasks;
using WeirdUnitBE.Middleware.Observable.ConcreteSubjects;

namespace WeirdUnitBE.Middleware.Observable
{
    public interface IObserver
    {
        Task SendData(ISubject subject, object data);
    }
}